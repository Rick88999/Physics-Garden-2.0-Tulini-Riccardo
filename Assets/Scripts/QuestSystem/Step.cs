using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Step : MonoBehaviour
{
    [SerializeField] private int stepID;
    [SerializeField] private string state = "noActivated";
    [SerializeField] private Quest father;
    [SerializeField] private Step NextStep;
    [Header("Oggetti che si attivano ad inizio step")]
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectsToDeactivateAtStart;
    [Header("Oggetti che appartengono allo step")]
    [SerializeField] private GameObject[] objectsList;
    [Header("Loot")]
    [SerializeField] private List<LootGiver> lootGivers = new();
    [SerializeField] private bool isLastStep;
    [SerializeField] private bool giveLootAtEndStep;
    [SerializeField] private ManifestManager manifestManager;

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
            foreach (GameObject objDeact in objectsToDeactivateAtStart)
            {
                objDeact.SetActive(false);
            }
            if (lootGivers != null && (!isLastStep && !giveLootAtEndStep))
            {
                if (lootGivers.Count > 0)
                    foreach (LootGiver loot in lootGivers)
                        loot.GiveLoot();
            }
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
            GiveLoot();
            manifestManager.questCompleted();
            father.endQuest();
        }
        else if (NextStep != null && !isLastStep)
        {
            if (giveLootAtEndStep) GiveLoot();
            manifestManager.stepCompleted(father.getIdQuest(), stepID);
            NextStep.startStep();
        }
    }

    public void setStateStep(string oldState)
    {
        if (oldState == "OnGoing") startStep();
        else state = oldState;
    }

    private void GiveLoot()
    {
        if (lootGivers != null)
        {
            if (lootGivers.Count > 0)
                foreach (LootGiver lootGiver in lootGivers)
                    lootGiver.GiveLoot();
        }
    }


    public string getStateStep() => state;
    public int getIdStep() => stepID;

}
