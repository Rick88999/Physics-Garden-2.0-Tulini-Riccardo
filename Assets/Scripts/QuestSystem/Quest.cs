using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] public int questID;
    [SerializeField] private List<Quest> alreadyComplete;
    [SerializeField] private string state = "noActivated";
    [SerializeField] public Step[] steps;
    [SerializeField] private StartCollider[] startColliders;
    [SerializeField] private ManifestManager manifestManager;
    [SerializeField] private GameObject dialogueMgError;


    public void startQuest()
    {
        state = "OnGoing";
        manifestManager.questStarted(questID);
        steps[0].startStep();
    }

    public void endQuest()
    {
        state = "Completed";

    }

    public void setQuestState(string oldState) => state = oldState;

    public int getIdQuest() => questID;

    public string getQuestState() => state;

    public bool allReadyComplete()
    {
        if (questID == 0) return true;
        else if ((alreadyComplete == null || alreadyComplete.Count == 0)) return false;

        foreach (Quest quest in alreadyComplete) if (quest.getQuestState() != "Completed") return false;
        return true;
    }



    public void PlayErrorMessage()
    {
        dialogueMgError.SetActive(true);
        ErrorDialogue dialogue = dialogueMgError.GetComponent<ErrorDialogue>();
        dialogue.StarterDialogueError();
    }
    public void setSavedStep(SavedStep[] savedSteps)
    {
        for (int i = 0; i < steps.Length; i++) steps[i].setStateStep(savedSteps[i].state); 
    }
    
    public SavedQuest getSavedQuest()
    {
        SavedQuest savedQuest = new SavedQuest();
        savedQuest.id_quest = questID;
        savedQuest.state = state;
        savedQuest.allSteps = new SavedStep[steps.Length];
        for (int i = 0; i < steps.Length; i++)
        {
            SavedStep savedStep = new SavedStep();
            savedStep.id_step = steps[i].getIdStep();
            savedStep.state = steps[i].getStateStep();
            savedQuest.allSteps[i] = savedStep;
        }
        return savedQuest;
    }
    public int GetLastOnGoingStep()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            if (steps[i].getStateStep() == "OnGoing")
            {
                return steps[i].getIdStep();
            }
        }
        return -1;
    }


    public void DestroyQuestTrigger()
    {
        if (startColliders == null || startColliders.Length == 0) return;
        else foreach (StartCollider startCollider in startColliders) startCollider.DestroyTriggers();
    }


}
