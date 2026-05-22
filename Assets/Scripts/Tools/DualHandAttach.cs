using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DualHandAttach : MonoBehaviour
{
    [SerializeField] private Transform attachRight;
    [SerializeField] private Transform attachLeft;
    [SerializeField] private InputActionReference scanActionRight;
    [SerializeField] private InputActionReference scanActionLeft;
    [SerializeField] private InputActionReference spawnCardActionRight;
    [SerializeField] private InputActionReference spawnCardActionLeft;
    private InputActionReference spawnCardAction;
    private InputActionReference scanAction;
    private bool? isRight = null;
    private bool? beforeGrabIsRight = null;
    private int counterDebug = 0;


    [SerializeField]private XRGrabInteractable grabbable;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerHand")) return;
        if (counterDebug == 0)
        {
            counterDebug++;
            return;
        }
        beforeGrabIsRight = isRight;

        isRight = other.transform.name.Contains("Right");
        grabbable.attachTransform = (bool)isRight ? attachRight : attachLeft;
        scanAction = (bool)isRight ? scanActionRight : scanActionLeft;
        spawnCardAction = (bool)isRight ? spawnCardActionRight : spawnCardActionLeft;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerHand")) return;
        if (beforeGrabIsRight == null)
        {
            isRight = null;
            grabbable.attachTransform = null;
            scanAction = null;
            spawnCardAction = null;
        }
        else if (beforeGrabIsRight != null)
        {
            isRight = beforeGrabIsRight;
            grabbable.attachTransform = (bool)isRight ? attachRight : attachLeft;
            scanAction = (bool)isRight ? scanActionRight : scanActionLeft;
            spawnCardAction = (bool)isRight ? spawnCardActionRight : spawnCardActionLeft;
        }
    }

    public InputActionReference getScanAction()
    {
        return scanAction;
    }

    public InputActionReference getSpawnCardAction()
    {
        return spawnCardAction;
    }

    /*void OnRelease(SelectExitEventArgs args)
    {
        // Resetta a null così XRI non riusa il vecchio attach
        grabbable.attachTransform = null;
    }*/

    /*public void ChangeHand(SelectEnterEventArgs args)
    {
        bool localIsRight = args.interactorObject.transform.name.Contains("Right");
        if (localIsRight != isRight)
        {
            isRight = localIsRight;
            grabbable.attachTransform = isRight ? attachRight : attachLeft;
            scanAction = isRight ? scanActionRight : scanActionLeft;
            spawnCardAction = isRight ? spawnCardActionRight : spawnCardActionLeft;
        }
    }*/

    /*void OnDestroy()
    {
 grabbable.selectEntered.RemoveListener(OnGrab);
    grabbable.selectExited.RemoveListener(OnRelease);
    }*/
}