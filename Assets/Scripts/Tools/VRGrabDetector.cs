using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRGrabDetector : MonoBehaviour
{
    [FormerlySerializedAs("grabbable")][FormerlySerializedAs("grab")]
    protected XRGrabInteractable grabbable;

    protected virtual void Awake()
    {
        grabbable = GetComponent<XRGrabInteractable>();
    }

    protected virtual void OnSelectEntered(SelectEnterEventArgs args) { }
    protected virtual void OnSelectExited(SelectExitEventArgs args) { }
}
