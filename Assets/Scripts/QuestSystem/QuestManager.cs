using System.Diagnostics;
using NUnit.Framework.Constraints;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class QuestManager : MonoBehaviour
{
    [Header("Quests")]
    [SerializeField] private Quest[] quests;
    [SerializeField] private GameObject EsterEggQuest;
    [Header("Manifest Manager")]
    [SerializeField] private ManifestManager manifestManager;

    [System.Serializable]
    private struct StartTriggersContainer
    {
        public GameObject[] startTriggers;
    }
    [Header("Trigger Controllers")]
    [SerializeField] private StartTriggersContainer[] allStartTriggers;

    private int currentQuest = 0;

    private void Start() => GameEvents.Instance.onQuestCompleted.AddListener(AllQuestsCompleted);

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
            else if (quest.getQuestState() == "Completed")
            {
                StartTriggersContainer startTriggerContainer = allStartTriggers[quest.getIdQuest()];
                foreach (GameObject startTrigger in startTriggerContainer.startTriggers) startTrigger.SetActive(false);
            }
        } 
    }

    public SavedQuest[] getSavedQuests()
    {
        SavedQuest[] savedQuests = new SavedQuest[quests.Length];
        for (int i = 0; i < quests.Length; i++) savedQuests[i] = quests[i].getSavedQuest();
        return savedQuests;
    }

    private void AllQuestsCompleted()
    {
        bool flag = true;

        foreach (Quest quest in quests) 
            if(quest.transform.gameObject.activeSelf) 
                if (quest.getQuestState() != "Completed") flag = false;

        if (flag) EsterEggQuest.SetActive(true);
    }
}
