using NUnit.Framework.Constraints;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Quest[] quests;
    [SerializeField] private ManifestManager manifestManager;

    [System.Serializable]
    private struct StartTriggersContainer
    {
        public GameObject[] startTriggers;
    }
    [SerializeField] private StartTriggersContainer[] allStartTriggers;

    private int currentQuest = 0;

    public bool startQuest(int number)
    {
        
        bool notOneOnGoing = true;
        
        foreach (Quest quest in quests) 
            if (quest.getQuestState() == "OnGoing" && quest.getIdQuest() != number) 
                notOneOnGoing = false;

        if (notOneOnGoing)
        {
            if (quests[number].allReadyComplete())
            {

                quests[number].startQuest();
                currentQuest = number;
               
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
            quests[i].AllCompleteSteps();
        }
        foreach (Quest quest in quests)
        {
            if (quest.getQuestState() == "OnGoing")
            {
                StartTriggersContainer startTriggerContainer = allStartTriggers[quest.getIdQuest()];
                foreach (GameObject startTrigger in startTriggerContainer.startTriggers) startTrigger.SetActive(false);
                int lastStep = quest.GetLastOnGoingStep();
                manifestManager.ReloadFromSavings(quest.getIdQuest(), lastStep);
            }
        } 
    }

    public SavedQuest[] getSavedQuests()
    {
        SavedQuest[] savedQuests = new SavedQuest[quests.Length];
        for (int i = 0; i < quests.Length; i++) savedQuests[i] = quests[i].getSavedQuest();
        return savedQuests;
    }
}
