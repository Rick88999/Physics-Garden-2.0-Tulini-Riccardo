using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class MassLaserScanner : MonoBehaviour
{
    public Transform rayOrigin;
    public float maxDistance = 10f;

    public Color coloreNormale = Color.red;
    public Color coloreScansionabile = Color.green;

    public InputActionReference scanAction;
    public float massaMemorizzata = 0f;

    private LineRenderer lineRenderer;
    [SerializeField] public TextMeshProUGUI displayRisultato;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        ImpostaColoreLaser(coloreNormale);
    }

    private void OnEnable()
    {
        if (scanAction != null) scanAction.action.performed += EseguiScansione;
    }

    private void OnDisable()
    {
        if (scanAction != null) scanAction.action.performed -= EseguiScansione;
    }

    private void Update()
    {
        if (rayOrigin == null) return;

        lineRenderer.SetPosition(0, rayOrigin.position);

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, maxDistance))
        {
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.attachedRigidbody != null)
            {
                ImpostaColoreLaser(coloreScansionabile);
            }
            else
            {
                ImpostaColoreLaser(coloreNormale);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin.position + rayOrigin.forward * maxDistance);

            ImpostaColoreLaser(coloreNormale);
        }
    }

    private void EseguiScansione(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (rayOrigin == null) return;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, maxDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                massaMemorizzata = rb.mass;

                displayRisultato.text = massaMemorizzata+" kg";
            }
        }
    }

 
    private void ImpostaColoreLaser(Color colore)
    {
        lineRenderer.startColor = colore;
        lineRenderer.endColor = colore;
    }
}