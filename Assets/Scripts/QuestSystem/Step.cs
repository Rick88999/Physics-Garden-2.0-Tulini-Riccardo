using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class Step : MonoBehaviour
{
    [SerializeField]private string state = "noActivated";
    [SerializeField] Quest father;
    [SerializeField] Step NextStep;
    [Header("Oggetti che si attivano ad inizio step")]
    [SerializeField] GameObject[] objectsToActivate;
    [Header("Oggetti che appartengono allo step")]
    [SerializeField] GameObject[] objectsList;
    [Header("Loot")]
    [SerializeField] LootGiver lootGiver;
    [SerializeField] bool isLastStep;

    // Update is called once per frame
    public void startStep()
    {
        if (state == "noActivated") 
        { 
            state = "OnGoing";
            foreach (GameObject objAct in objectsToActivate)
            {
                objAct.SetActive(true);
            }
            if(lootGiver!=null) lootGiver.GiveLoot();
        }
        else
        {
            Debug.Log("This step is already activated");
        }
    }

    public void endStep()
    {
        state = "Completed";
        foreach (GameObject obj in objectsList)
        {
            obj.SetActive(false);
        }

        if(NextStep!=null) NextStep.startStep();
        else if (isLastStep)
        {
            father.endQuest();
        }
        
    }

    public void setStateStep(string oldState)
    {
        if (oldState == "OnGoing") startStep();
        else state = oldState;
    }


    public string getStateStep() => state;

}
