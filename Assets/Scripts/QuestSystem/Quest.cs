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

    public void setQuestState(string oldState)
    {
        state = oldState;
    }

    public string getQuestState()
    {
        return state;
    }

    public void DestroyQuestTrigger()
    {
        if (startColliders == null || startColliders.Length == 0) return;
        else foreach (StartCollider startCollider in startColliders) startCollider.DestroyTriggers();
    }


}
