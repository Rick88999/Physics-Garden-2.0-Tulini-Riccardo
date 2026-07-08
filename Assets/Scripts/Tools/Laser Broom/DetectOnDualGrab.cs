using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class DetectOnDualGrab : MonoBehaviour
{
    private XRGrabInteractable grab;
    private bool isGrabbedRight = false;
    private bool isGrabbedLeft = false;
    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        var handedness = args.interactorObject.transform.GetComponentInParent<ManoEco>();
        if (handedness == null) return;

        if (handedness.hand == Hand.Right)
            isGrabbedRight = true;
        else if (handedness.hand == Hand.Left)
            isGrabbedLeft = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        var handedness = args.interactorObject.transform.GetComponentInParent<ManoEco>();
        if (handedness == null) return;
        
        if (handedness.hand == Hand.Right)
            isGrabbedRight = false;
        else if (handedness.hand == Hand.Left)
            isGrabbedLeft = false;
    }

    public bool IsGrabbedByBothHands()
    {
        return isGrabbedRight && isGrabbedLeft;
    }
}
