using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(XRSimpleInteractable))]
[RequireComponent(typeof(MeshCollider))]
public class VRInterruptor : MonoBehaviour
{
    public class GraphEvent
    {
        public UnityEvent eventIO;
        public GraphEvent nextEvent;
    }

    [SerializeField] private UnityEvent[] onPressed = new UnityEvent[2];
    private GraphEvent on = new GraphEvent();
    private GraphEvent off = new GraphEvent();
    private GraphEvent actualEvent = new GraphEvent();


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

        on.eventIO = onPressed[0];
        off.eventIO = onPressed[1];
        on.nextEvent = off;
        off.nextEvent = on;
        actualEvent = on;

        interactable.hoverEntered.AddListener(OnPress);
        interactable.hoverExited.AddListener(OnRelease);
    }

    void Start() => startPosition = transform.localPosition;

    private void OnPress(HoverEnterEventArgs args)
    {
        if (isPressed || inCooldown) return;

        isPressed = true;

        transform.localPosition = startPosition - new Vector3(0, 0.005f, 0);

        actualEvent.eventIO.Invoke();
    }

    private void OnRelease(HoverExitEventArgs args)
    {
        if (!isPressed) return;

        isPressed = false;
        transform.localPosition = startPosition;
        actualEvent = actualEvent.nextEvent;

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        inCooldown = true; 
        yield return new WaitForSeconds(cooldownDelay); 
        inCooldown = false;
    }
}
