using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float ejectForce = 3f;
    [SerializeField] private Vector3 ejectDirection = Vector3.forward;
    [SerializeField] private List<Collider> box;



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
                        StartCoroutine(DestroyCard(interactable));
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
        yield return new WaitForSeconds(socketDisableDuration);
        audioSource.PlayOneShot(soundError);
        IXRSelectInteractable interactable = socketInteractor.GetOldestInteractableSelected();
        if (interactable != null)
        {
            socketInteractor.interactionManager.SelectCancel(socketInteractor, interactable);
            socketInteractor.socketActive = false;
            if (interactable.transform.TryGetComponent(out Rigidbody rb) && interactable.transform.TryGetComponent(out Collider cardCollider))
            {
                foreach (var collider in box)
                {
                    Physics.IgnoreCollision(cardCollider, collider, true);
                }
                rb.AddForce(ejectDirection.normalized * ejectForce, ForceMode.Impulse);
                StartCoroutine(RestoreCollision(cardCollider));
            }
        }
        socketInteractor.socketActive = false;
        yield return new WaitForSeconds(socketDisableDuration);
        socketInteractor.socketActive = true;
    }

    private IEnumerator RestoreCollision(Collider cardCollider)
    {
        yield return new WaitForSeconds(0.5f);
        if (cardCollider != null)
        {
            foreach (var collider in box)
            {
                Physics.IgnoreCollision(cardCollider, collider, false);
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
