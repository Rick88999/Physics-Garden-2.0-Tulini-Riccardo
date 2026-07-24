using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Linq;

[RequireComponent(typeof(MemoryValue))]
public class DataCardScanValueActivator : CardReader
{
    [Header("Riferimento allo step in corso")]
    [SerializeField] private Step Step;
    [SerializeField] private float stepCallDelay = 3.5f;
    private MemoryValue memoryValue;

    private void Awake()
    {
        MemoryValue memValue = GetComponent<MemoryValue>();

        if (memValue != null)
        {
            memoryValue = memValue;
        }
        else
        {
            Debug.LogWarning("Il GameObject " + memoryValue.name + " non ha il componente MemoryValue.");
        }
    }

    public override void ActivateChecks()
    {
        string name;
        float value;
        if (socketInteractor != null)
        {
            IXRSelectInteractable interactable = socketInteractor.GetOldestInteractableSelected();
            if (interactable != null)
            {
                DataCard dataCardValue = interactable.transform.GetComponent<DataCard>();
                if (dataCardValue != null)
                {
                    name = dataCardValue.GetName();
                    value = dataCardValue.GetValue();
                    if (memoryValue != null)
                    {
                        if (memoryValue.ControlValue(name, value))
                        {
                           if (audioSource != null && clickSoundSuccess != null)
                            {
                                audioSource.PlayOneShot(clickSoundSuccess);
                            }
                            StartCoroutine(EndStep());
                        }
                        else
                        {
                            StartCoroutine(CardEjection(interactable));
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Il componente MemoryValue non è stato trovato.");
                    }
                }
            }
        }
        
    }

    private IEnumerator EndStep()
    {
        yield return new WaitForSeconds(stepCallDelay);
        Step.endStep();
    }

}

    

