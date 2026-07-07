using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class EsterEggCardReader : MonoBehaviour
{
    [SerializeField] private Step step;
    [SerializeField] private XRSocketInteractor socketInteractorRed;
    [SerializeField] private XRSocketInteractor socketInteractorGreen;
    [SerializeField] private XRSocketInteractor socketInteractorBlue;
    [SerializeField] private GameObject EsterEggPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundSuccess;

    private bool isBlueCardInserted = false;
    private bool isRedCardInserted = false;
    private bool isGreenCardInserted = false;

    private void Update()
    {
        if (isBlueCardInserted && isRedCardInserted && isGreenCardInserted)
        {
            EsterEggPrefab.SetActive(true);
            IXRSelectInteractable interactableRed = socketInteractorRed.GetOldestInteractableSelected();
            IXRSelectInteractable interactableBlue = socketInteractorBlue.GetOldestInteractableSelected();
            IXRSelectInteractable interactableGreen = socketInteractorGreen.GetOldestInteractableSelected();
            Destroy(interactableRed.transform.gameObject);
            Destroy(interactableBlue.transform.gameObject);
            Destroy(interactableGreen.transform.gameObject);
            step.endStep();
            audioSource.PlayOneShot(soundSuccess);
            isBlueCardInserted = false;
            isRedCardInserted = false;
            isGreenCardInserted = false;
            gameObject.SetActive(false);
        }
    }

    public void RedCardCheck() => isRedCardInserted = true;
    public void BlueCardCheck() => isBlueCardInserted = true;
    public void GreenCardCheck() => isGreenCardInserted = true;
}
