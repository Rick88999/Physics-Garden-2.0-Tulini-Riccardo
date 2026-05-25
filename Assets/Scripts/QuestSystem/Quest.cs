using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] public int questID;
    [SerializeField]private string state = "noActivated";
    [SerializeField] public Step[] steps;
    [SerializeField] private StartCollider[] startColliders;


    public void startQuest()
    {
        state = "OnGoing";
        steps[0].startStep();
    }

    public void endQuest()
    {
        state = "Completed";

    }

    public void setQuestState(string oldState) => state = oldState;

    public int getIdQuest() => questID;

    public string getQuestState() => state;

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
            savedStep.state = steps[i].getStateStep();
            savedQuest.allSteps[i] = savedStep;
        }
        return savedQuest;
    }


    public void DestroyQuestTrigger()
    {
        if (startColliders == null || startColliders.Length == 0) return;
        else foreach (StartCollider startCollider in startColliders) startCollider.DestroyTriggers();
    }


}
