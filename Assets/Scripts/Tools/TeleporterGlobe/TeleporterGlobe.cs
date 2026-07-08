using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class TeleporterGlobe : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    private DetectHandOnGrab detectHandOnGrab;
    private InputActionReference inputTeleport;

    [Header("Cone Settings")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float coneAngle = 15f;
    [SerializeField] private LayerMask targetLayers;

    [Header("Debug Gizmos")]
    //[SerializeField] private bool drawGizmos = true;
    //[SerializeField] private int gizmoSegments = 24;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;
    private LookTarget currentTarget;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        detectHandOnGrab = GetComponent<DetectHandOnGrab>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
            GameEvents.Instance.onChangeHandTeleportTool.AddListener(OnChangeHand);
            GameEvents.Instance.onDropTeleportTool.AddListener(OnDrop);
        }
    }

    private void RebindActions()
    {
        if(inputTeleport != null)
            inputTeleport.action.performed -= OnInputPerformed;
        
        inputTeleport = detectHandOnGrab.GetPrimaryAction();

        if (isActiveAndEnabled && inputTeleport != null)
        {
            inputTeleport.action.performed += OnInputPerformed;
            // L'action è condivisa tra tutti i tool della stessa mano:
            // riabilitarla qui recupera eventuali Disable() fatti altrove.
            inputTeleport.action.Enable();
        }
    }

    private void OnEnable() => RebindActions();

    private void OnDisable()
    {
        // Solo unsubscribe: niente Disable(), l'action è condivisa
        // e spegnerla bloccherebbe l'input di tutti gli altri tool.
        if (inputTeleport != null && inputTeleport.action != null)
            inputTeleport.action.performed -= OnInputPerformed;
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.onChangeHandTeleportTool.RemoveListener(OnChangeHand);
            GameEvents.Instance.onDropTeleportTool.RemoveListener(OnDrop);
        }
    }


    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        RebindActions();
    }

    private void OnChangeHand(string newHand, int idRiser)
    {
        if (gameObject.GetInstanceID() == idRiser) RebindActions();
    }

    private void OnDrop(int idRiser)
    {
        if (gameObject.GetInstanceID() == idRiser) RebindActions();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        if (currentTarget != null)
        {
            if (GameEvents.Instance != null)
                GameEvents.Instance.onDefocusTeleport.Invoke(currentTarget.GetTeleporterName());
            currentTarget = null;
        }
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (!isHeld) return;

        if (currentTarget != null)
            currentTarget.Activate();
        
    }

    void Update()
    {
        if (!isHeld) return;
        if (cam == null) return;

        LookTarget newTarget = FindBestTargetInCone();

        if (newTarget != currentTarget)
        {
            if (currentTarget != null && GameEvents.Instance != null)
                GameEvents.Instance.onDefocusTeleport.Invoke(currentTarget.GetTeleporterName());

            currentTarget = newTarget;

            if (currentTarget != null && GameEvents.Instance != null)
                GameEvents.Instance.onTargetTeleport.Invoke(currentTarget.GetTeleporterName());
        }
    }

    private LookTarget FindBestTargetInCone()
    {
        Collider[] hits = Physics.OverlapSphere(cam.transform.position, maxDistance, targetLayers);

        LookTarget bestTarget = null;
        float bestAngle = coneAngle;

        foreach (var col in hits)
        {
            LookTarget target = col.GetComponent<LookTarget>();
            if (target == null) continue;

            Vector3 toTarget = target.transform.position - cam.transform.position;
            float distance = toTarget.magnitude;

            if (distance > maxDistance)
                continue;

            float angle = Vector3.Angle(cam.transform.forward, toTarget.normalized);

            if (angle <= coneAngle && angle < bestAngle)
            {
                bestAngle = angle;
                bestTarget = target;
            }
        }

        return bestTarget;
    }

    /*private void OnDrawGizmos()
    {
        if (!drawGizmos || cam == null) return;

        Vector3 origin = cam.transform.position;
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;
        Vector3 up = cam.transform.up;

        float radius = Mathf.Tan(coneAngle * Mathf.Deg2Rad) * maxDistance;
        Vector3 endCenter = origin + forward * maxDistance;

        Gizmos.color = isHeld ? Color.green : Color.red;

        // linea centrale
        Gizmos.DrawLine(origin, endCenter);

        // cerchio finale del cono
        Vector3 prev = endCenter + right * radius;
        for (int i = 1; i <= gizmoSegments; i++)
        {
            float ang = (float)i / gizmoSegments * Mathf.PI * 2f;
            Vector3 offset = (right * Mathf.Cos(ang) + up * Mathf.Sin(ang)) * radius;
            Vector3 next = endCenter + offset;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }

        // linee che collegano l'origin al bordo del cerchio
        for (int i = 0; i < gizmoSegments; i += 4)
        {
            float ang = (float)i / gizmoSegments * Mathf.PI * 2f;
            Vector3 offset = (right * Mathf.Cos(ang) + up * Mathf.Sin(ang)) * radius;
            Gizmos.DrawLine(origin, endCenter + offset);
        }
    }*/
}
