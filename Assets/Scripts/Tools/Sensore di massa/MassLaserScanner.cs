using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class MassLaserScanner : VRToolOneHand
{
    [SerializeField] private SpawnCard spawnCard;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float maxDistance = 10f;

    [SerializeField] private Color coloreNormale = Color.red;
    [SerializeField] private Color coloreScansionabile = Color.green;

    private LineRenderer lineRenderer;
    private float massaMemorizzata = 0f;
    
    [SerializeField] private TextMeshProUGUI displayRisultato;

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
        spawnCard = GetComponent<SpawnCard>();
        
        if (lineRenderer != null)
            lineRenderer.positionCount = 2;

        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.onChangeHandMassTool.AddListener(OnChangeHand);
            GameEvents.Instance.onDropMassTool.AddListener(OnDrop);
        }

        ImpostaColoreLaser(coloreNormale);
    }

    protected override void SubscribeActions()
    {
        if (lineRenderer != null) lineRenderer.enabled = true;

        if (isActiveAndEnabled)
        {
            if (primaryAction != null) primaryAction.action.performed += GenerateDataCard;
            if (secondaryAction != null) secondaryAction.action.performed += EseguiScansione;
        }
    }

    protected override void UnsubscribeActions()
    {
        if (primaryAction != null) primaryAction.action.performed -= GenerateDataCard;
        if (secondaryAction != null) secondaryAction.action.performed -= EseguiScansione;
    }

    protected override void UnsubscribeEvents()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.onChangeHandMassTool.RemoveListener(OnChangeHand);
            GameEvents.Instance.onDropMassTool.RemoveListener(OnDrop);
        }
    }

    protected override void OnChangeHand(string newHand, int idRiser)
    { 
        if (gameObject.GetInstanceID() == idRiser) RebindActions();
    }

    protected override void OnDrop(int idRiser)
    {
        if (gameObject.GetInstanceID() == idRiser)
        {
            RebindActions();
            if (lineRenderer != null) lineRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (rayOrigin == null || lineRenderer == null) return;

        lineRenderer.SetPosition(0, rayOrigin.position);

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, maxDistance))
        {
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.attachedRigidbody != null)
                ImpostaColoreLaser(coloreScansionabile);
            else
                ImpostaColoreLaser(coloreNormale);
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin.position + rayOrigin.forward * maxDistance);
            ImpostaColoreLaser(coloreNormale);
        }
    }

    private void EseguiScansione(InputAction.CallbackContext context)
    {
        if (!context.performed || rayOrigin == null) return;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, maxDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                massaMemorizzata = rb.mass;
                if (displayRisultato != null) displayRisultato.text = massaMemorizzata + "kg";
                Debug.Log("Massa scansionata: " + massaMemorizzata + " kg");
            }
        }
    }

    private void GenerateDataCard(InputAction.CallbackContext context) 
    { 
        if (!context.performed || spawnCard == null || massaMemorizzata <= 0f) return;

        spawnCard.ValueReceiver(massaMemorizzata);
        spawnCard.SpawnDataCard();
    }

    private void ImpostaColoreLaser(Color colore)
    {
        if (lineRenderer == null) return;
        lineRenderer.startColor = colore;
        lineRenderer.endColor = colore;
    }
}