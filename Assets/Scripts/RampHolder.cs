using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RampHolder : MonoBehaviour
{
    private XRSocketInteractor socket;

    void Awake() => socket = GetComponent<XRSocketInteractor>();

    public void Release()
    {
        if (socket.firstInteractableSelected != null)
        {
            socket.interactionManager.SelectExit(
                socket,
                socket.firstInteractableSelected
            );

            StartCoroutine(DisableSocket());
        }
    }

    IEnumerator DisableSocket()
    {
        socket.enabled = false;
        yield return new WaitForSeconds(3f);
        socket.enabled = true;
    }
}
