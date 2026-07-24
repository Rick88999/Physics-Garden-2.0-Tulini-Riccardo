using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public abstract class CardReader : MonoBehaviour
{
    [Header("Socket Interactors")]
    [SerializeField] protected XRSocketInteractor socketInteractor;
    [Header("Ejection Settings")]
    [SerializeField] protected float socketDisableDuration = 1f;
    [SerializeField] protected float ejectForce = 3f;
    [SerializeField] protected EjectDirectionAxis ejectDirectionAxis = EjectDirectionAxis.Forward;
    [SerializeField] protected List<Collider> box;
    [Header("Audio")]
    [SerializeField] protected AudioSource audioSource;
    [FormerlySerializedAs("soundError")][FormerlySerializedAs("clickSoundError")]
    [SerializeField] protected AudioClip clickSoundError;
    [FormerlySerializedAs("soundSuccess")][FormerlySerializedAs("clickSoundSuccess")]
    [SerializeField] protected AudioClip clickSoundSuccess;

    public abstract void ActivateChecks();

    protected virtual IEnumerator CardEjection(IXRSelectInteractable interactable)
    {
        yield return new WaitForSeconds(socketDisableDuration);
        audioSource.PlayOneShot(clickSoundError);
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
                rb.AddForce(GetEjectDirection() * ejectForce, ForceMode.Impulse);
                StartCoroutine(RestoreCollision(cardCollider));
            }
        }
        socketInteractor.socketActive = false;
        yield return new WaitForSeconds(socketDisableDuration);
        socketInteractor.socketActive = true;
    }
    protected virtual IEnumerator RestoreCollision(Collider cardCollider)
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
    protected virtual Vector3 GetEjectDirection()
    {
        switch (ejectDirectionAxis)
        {
            case EjectDirectionAxis.Forward: return transform.forward;
            case EjectDirectionAxis.Back: return -transform.forward;
            case EjectDirectionAxis.Right: return transform.right;
            case EjectDirectionAxis.Left: return -transform.right;
            case EjectDirectionAxis.Up: return transform.up;
            case EjectDirectionAxis.Down: return -transform.up;
            default: return transform.forward;
        }
    }

}
