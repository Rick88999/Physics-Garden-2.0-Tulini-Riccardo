using System.Collections; // Necessario per le Coroutine
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRButton : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent onPressed;

    [Header("Impostazioni Pulsante")]
    [Tooltip("Secondi da aspettare dopo il rilascio prima di poter premere di nuovo")]
    [SerializeField] private float cooldownDelay = 2.0f;

    private XRSimpleInteractable interactable;
    private Vector3 startPosition;
    private bool isPressed = false;
    private bool inCooldown = false; 

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.AddListener(OnPress);
        interactable.hoverExited.AddListener(OnRelease);
    }

    void Start() => startPosition = transform.localPosition;

    private void OnPress(HoverEnterEventArgs args)
    {
        if (isPressed || inCooldown) return;

        isPressed = true;
        transform.localPosition = startPosition - new Vector3(0, 0.005f, 0);
        onPressed.Invoke();
    }

    private void OnRelease(HoverExitEventArgs args)
    {
        
        if (!isPressed) return;

        isPressed = false;
        transform.localPosition = startPosition;
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        inCooldown = true;
        yield return new WaitForSeconds(cooldownDelay);
        inCooldown = false;
    }
}
