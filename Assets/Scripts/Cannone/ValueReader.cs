using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ValueReader : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socketInteractor;
    [SerializeField] private CannoneController cannone;
    [SerializeField] private string unitaMisura;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip soundError;

    [SerializeField] private float socketDisableDuration = 1f;

    public void ActivateCheckValue()
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
                        cannone.SetLaunchForceCannon(dataCard.GetValue());
                    }
                    else
                    {
                        StartCoroutine(DisableSocketTemporarily());
                    }
                }
                else
                {
                    StartCoroutine(DisableSocketTemporarily());
                }
            }
        }
    }

    private IEnumerator DisableSocketTemporarily()
    {
        yield return new WaitForSeconds(socketDisableDuration/2);
        audioSource.PlayOneShot(soundError);
        socketInteractor.socketActive = false;
        yield return new WaitForSeconds(socketDisableDuration);
        socketInteractor.socketActive = true;
    }
}
