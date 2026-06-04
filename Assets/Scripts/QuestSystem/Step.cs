using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    [SerializeField]private int stepID;
    [SerializeField]private string state = "noActivated";
    [SerializeField] Quest father;
    [SerializeField] Step NextStep;
    [Header("Oggetti che si attivano ad inizio step")]
    [SerializeField] GameObject[] objectsToActivate;
    [Header("Oggetti che appartengono allo step")]
    [SerializeField] GameObject[] objectsList;
    [Header("Loot")]
    [SerializeField] List<LootGiver> lootGivers = new();
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
            if (lootGivers != null && !isLastStep)
            {
                if (lootGivers.Count > 0)
                    foreach (LootGiver loot in lootGivers)
                        loot.GiveLoot();
            }
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

        if (isLastStep)
        {
            if (lootGivers != null)
            {
                if(lootGivers.Count > 0)
                    foreach (LootGiver lootGiver in lootGivers)
                        lootGiver.GiveLoot();
            }
            father.endQuest();
        }
        else if(NextStep!=null && !isLastStep) NextStep.startStep();
    }

    public void setStateStep(string oldState)
    {
        if (oldState == "OnGoing") startStep();
        else state = oldState;
    }


    public string getStateStep() => state;
    public int getIdStep() => stepID;

}
