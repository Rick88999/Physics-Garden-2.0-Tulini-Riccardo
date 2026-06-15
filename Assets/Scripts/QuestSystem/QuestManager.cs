using NUnit.Framework.Constraints;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] Quest[] quests;
    private int currentQuest = 0;

    public bool startQuest(int number)
    {
        currentQuest = number;
        bool notOneOnGoing = true;
        foreach (Quest quest in quests) if (quest.getQuestState() == "OnGoing") notOneOnGoing = false;
        if (notOneOnGoing)
        {
            if (quests[number].allReadyComplete())
            {
                quests[number].startQuest();
                return true;
            }
            else
            {
                quests[number].PlayErrorMessage();
                return false;
            }
        }
        else
        {
            quests[number].PlayErrorMessage();
            return false;
        }
        
    }

    public void settingsSaves(SavedQuest[] allQuest)
    {
        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].setQuestState(allQuest[i].state);
            quests[i].setSavedStep(allQuest[i].allSteps);
        }
    }

    public SavedQuest[] getSavedQuests()
    {
        SavedQuest[] savedQuests = new SavedQuest[quests.Length];
        for (int i = 0; i < quests.Length; i++) savedQuests[i] = quests[i].getSavedQuest();
        return savedQuests;
    }
}
