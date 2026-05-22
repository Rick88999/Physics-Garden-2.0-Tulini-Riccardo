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
    

    [SerializeField]private XRGrabInteractable grabbable;

    void OnTriggerEnter(Collider other)
    {
        bool isRight = other.transform.name.Contains("Right");
        grabbable.attachTransform = isRight ? attachRight : attachLeft;
        scanAction = isRight ? scanActionRight : scanActionLeft;
        spawnCardAction = isRight ? spawnCardActionRight : spawnCardActionLeft;
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

    void OnDestroy()
    {
        //grabbable.selectEntered.RemoveListener(OnGrab);
        //grabbable.selectExited.RemoveListener(OnRelease);
    }
}