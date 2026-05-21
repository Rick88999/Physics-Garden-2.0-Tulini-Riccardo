using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DualHandAttach : MonoBehaviour
{
    [SerializeField] private Transform attachRight;
    [SerializeField] private Transform attachLeft;

    [SerializeField]private XRGrabInteractable grabbable;

    void OnTriggerEnter(Collider other)
    {
        // Controlla se è mano destra o sinistra
        bool isRight = other.transform.name.Contains("Right");
        grabbable.attachTransform = isRight ? attachRight : attachLeft;
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