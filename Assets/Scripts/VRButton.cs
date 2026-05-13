using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRButton : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent onPressed;

    private XRSimpleInteractable interactable;
    private Vector3 startPosition;
    private bool isPressed = false;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.AddListener(OnPress);
        interactable.hoverExited.AddListener(OnRelease);
    }

    void Start() => startPosition = transform.localPosition;

    private void OnPress(HoverEnterEventArgs args)
    {
        if (isPressed) return;
        isPressed = true;

        transform.localPosition = startPosition - new Vector3(0, 0.005f, 0);

        onPressed.Invoke();
    }

    private void OnRelease(HoverExitEventArgs args)
    {
        isPressed = false;
        transform.localPosition = startPosition;
    }
}
