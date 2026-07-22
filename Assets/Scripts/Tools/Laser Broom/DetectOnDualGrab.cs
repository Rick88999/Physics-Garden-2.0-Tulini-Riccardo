using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class DetectOnDualGrab : VRGrabDetector
{
    private bool isGrabbedRight = false;
    private bool isGrabbedLeft = false;
    protected override void Awake()
    {
        base.Awake();
        grabbable.selectEntered.AddListener(OnSelectEntered);
        grabbable.selectExited.AddListener(OnSelectExited);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        ManoEco handedness = args.interactorObject.transform.GetComponentInParent<ManoEco>();
        if (handedness == null) return;

        if (handedness.hand == Hand.Right)
            isGrabbedRight = true;
        else if (handedness.hand == Hand.Left)
            isGrabbedLeft = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        ManoEco handedness = args.interactorObject.transform.GetComponentInParent<ManoEco>();
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
