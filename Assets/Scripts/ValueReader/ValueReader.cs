using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ValueReader : CardReader
{

    [SerializeField] private Handler<float> handler;
    [SerializeField] private string unitaMisura;

    public override void ActivateChecks()
    {
        if (socketInteractor != null)
        {
            IXRSelectInteractable interactable = socketInteractor.GetOldestInteractableSelected();
            if (interactable != null)
            {
                DataCard dataCard = interactable.transform.GetComponent<DataCard>();
                if (dataCard != null)
                {
                    if (dataCard.GetUnitaMisura() == unitaMisura)
                    {
                        handler.Perform(dataCard.GetValue());
                        StartCoroutine(DestroyCard(interactable));
                    }
                    else
                    {
                        StartCoroutine(CardEjection(interactable));
                    }
                }
                else
                {
                    StartCoroutine(CardEjection(interactable));
                }
            }
        }
    }
    
    private IEnumerator DestroyCard(IXRSelectInteractable interactable)
    {
        yield return new WaitForSeconds(0.5f);
        if (interactable != null)
            Destroy(interactable.transform.gameObject);
    }
}
