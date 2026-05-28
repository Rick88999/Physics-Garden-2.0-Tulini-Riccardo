using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Linq;


public class DataCardScanValueActivator : MonoBehaviour
{
    [Header("Riferimento allo step in corso")]
    [SerializeField] private Step Step;
    [SerializeField] private List<GameObject> memory;
    private List<MemoryValue> memoryValueList = new List<MemoryValue>();
    [Header("Socket Interactors")]
    [SerializeField] private int socketPreviste;
    [SerializeField] private GameObject[] sockets;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSoundError;
    [SerializeField] private AudioClip clickSoundSuccess;

    [SerializeField] private List<XRSocketInteractor> socketInteractors;
    [SerializeField] private List<IXRSelectInteractable> interactables = new List<IXRSelectInteractable>();

    private void Awake()
    {
        foreach (GameObject mem in memory)
        {
            if (mem != null)
            {
                MemoryValue memValue = mem.GetComponent<MemoryValue>();
                if (memValue != null)
                {
                    memoryValueList.Add(memValue);
                }
                else
                {
                    Debug.LogWarning("Il GameObject " + mem.name + " non ha il componente MemoryValue.");
                }
            }
            else
            {
                Debug.LogWarning("Uno dei GameObject nella lista 'memory' non è assegnato.");
            }
        }
    }
    private void Control(bool ischecked)
    {
        
        if (ischecked)
        {
            if (audioSource != null && clickSoundSuccess != null)
            {
                audioSource.PlayOneShot(clickSoundSuccess);
            }
            StartCoroutine(EndStep());
        }
        else
        {
            for (int i = 0; i < socketInteractors.Count; i++)
            {
                RampHolder socketHolder = sockets[i].GetComponent<RampHolder>();

                if (socketHolder != null)
                {
                    socketHolder.Release();
                }
            }
            interactables.Clear();

            if (audioSource != null && clickSoundError != null)
            {
                audioSource.PlayOneShot(clickSoundError);
            }
            Debug.LogWarning("Valore o nome non corrispondente. Step non attivato.");
        }

    }

    public void ActiavateChecks()
    {
        foreach (XRSocketInteractor socket in socketInteractors)
        {
            if (socket != null)
            {
                IXRSelectInteractable interactable = socket.GetOldestInteractableSelected();
                if (interactable != null)
                {
                    if(!interactables.Contains(interactable))
                    {
                        interactables.Add(interactable);
                    }
                }
            }
        }

        if(interactables.Count == socketPreviste)
        {
            string name;
            float value;
            bool[] checks = new bool[socketPreviste];

            for (int i = 0; i < interactables.Count; i++)
            {
                DataCard dataCardValue = interactables[i].transform.GetComponent<DataCard>();
                if (dataCardValue != null)
                {
                    bool esito = false;
                    name = dataCardValue.GetName();
                    value = dataCardValue.GetValue();
                    foreach (MemoryValue memValue in memoryValueList)
                    {
                        if (memValue.ControlValue(name, value))
                        {
                            esito = true;
                            checks[i] = esito;
                            break;
                        }
                    }
                }
                else Debug.LogWarning($"L'interactable {i} non ha il componente DataCardValue.");
            }

            if(checks.All(check => check == true))
            {
                Control(true);
            }
            else
            {
                Control(false);
            }

        }
        else
        {
            interactables.Clear();
            Debug.Log("Non sono state inserite tutte le schede dati previste.");
        }
    }

    IEnumerator EndStep()
    {
        yield return new WaitForSeconds(3.5f);
        Step.endStep();
    }

    
}
