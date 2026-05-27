using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;

public struct GraphEvents
{
    public UnityEvent event;
    public UnityEvent nextEvent;
}

public class VRInterruptor : MonoBehaviour
{
    [SerializeField] private UnityEvent[] onPressed = new UnityEvent[2];
    private GraphEvents graphEvents = new GraphEvents();


    [Header("Impostazioni Pulsante")]
    [Tooltip("Secondi da aspettare dopo il rilascio prima di poter premere di nuovo")]
    [SerializeField] private float cooldownDelay = 2.0f;

    private XRSimpleInteractable interactable;
    private Vector3 startPosition;
    private bool isPressed = false;
    private bool inCooldown = false; // Flag per bloccare lo spam

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        interactable.hoverEntered.AddListener(OnPress);
        interactable.hoverExited.AddListener(OnRelease);
    }

    void Start() => startPosition = transform.localPosition;

    private void OnPress(HoverEnterEventArgs args)
    {
        // Se il pulsante è già premuto o stiamo aspettando il timer di cooldown, ignora l'input
        if (isPressed || inCooldown) return;

        isPressed = true;

        // Abbassa il pulsante visivamente
        transform.localPosition = startPosition - new Vector3(0, 0.005f, 0);

        // Genera la carta (o qualsiasi altro evento)
        onPressed.Invoke();
    }

    private void OnRelease(HoverExitEventArgs args)
    {
        // Se si esce dall'area ma il pulsante non era stato effettivamente premuto (es. durante il cooldown), ignora
        if (!isPressed) return;

        // Resetta lo stato di pressione e rialza il pulsante
        isPressed = false;
        transform.localPosition = startPosition;


        // Avvia il timer di pausa prima che possa essere premuto di nuovo
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        inCooldown = true; // Blocca il pulsante
        yield return new WaitForSeconds(cooldownDelay); // Aspetta i secondi scelti nell'Inspector
        inCooldown = false; // Sblocca il pulsante
    }
}
