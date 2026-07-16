using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RampHolder : MonoBehaviour
{
    private XRSocketInteractor socket;
    private string layerMask = "OggettoPianoInclinatoInMoto";

    void Awake() => socket = GetComponent<XRSocketInteractor>();

    public void Release()
    {
        if (socket.hasSelection)
        {
           GameObject selectedObject = socket.GetOldestInteractableSelected().transform.gameObject;
           if (selectedObject != null) selectedObject.GetComponent<XRGrabInteractable>().enabled = false;
           selectedObject.layer = LayerMask.NameToLayer(layerMask);
        }
    }

   
}
