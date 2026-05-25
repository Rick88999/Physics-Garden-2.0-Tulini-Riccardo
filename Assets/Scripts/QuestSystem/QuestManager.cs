using NUnit.Framework.Constraints;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] Quest[] quests;
    private int currentQuest = 0;

    public void startQuest(int number)
    {
        currentQuest = number;
        quests[number].startQuest();
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
