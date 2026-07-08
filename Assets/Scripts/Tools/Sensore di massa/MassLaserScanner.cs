using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class MassLaserScanner : MonoBehaviour
{
    [SerializeField] private SpawnCard spawnCard;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float maxDistance = 10f;

    [SerializeField] private Color coloreNormale = Color.red;
    [SerializeField] private Color coloreScansionabile = Color.green;

    private DetectHandOnGrab detectHandOnGrab;
    [SerializeField]private InputActionReference scanAction;
    [SerializeField] private InputActionReference spawnCardAction;
    private LineRenderer lineRenderer;

    private float massaMemorizzata = 0f;
    
    [SerializeField] private TextMeshProUGUI displayRisultato;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        detectHandOnGrab = GetComponent<DetectHandOnGrab>();
        spawnCard = GetComponent<SpawnCard>();
        lineRenderer.positionCount = 2;

        GameEvents.Instance.onChangeHandMassTool.AddListener(OnChangeHand);
        GameEvents.Instance.onDropMassTool.AddListener(OnDrop);

        ImpostaColoreLaser(coloreNormale);
    }

    private void RebindActions()
    {
        lineRenderer.enabled = true;
        if (spawnCardAction != null)
            spawnCardAction.action.performed -= GenerateDataCard;
        if (scanAction != null)
            scanAction.action.performed -= EseguiScansione;
        


        spawnCardAction = detectHandOnGrab.GetPrimaryAction();
        scanAction = detectHandOnGrab.GetSecondaryAction();

        
        if (isActiveAndEnabled && spawnCardAction != null)
            spawnCardAction.action.performed += GenerateDataCard;
        if (isActiveAndEnabled && scanAction != null)
            scanAction.action.performed += EseguiScansione;
    }

    private void OnChangeHand(string newHand, int idRiser)
    { 
        if(gameObject.GetInstanceID() == idRiser) RebindActions();
    }

    private void OnDrop(int idRiser)
    {
        if (gameObject.GetInstanceID() == idRiser)
        {
            RebindActions();
            lineRenderer.enabled = false;
        }
    }

    private void OnEnable() => RebindActions();

    private void OnDisable()
    {
        if (spawnCardAction != null) spawnCardAction.action.performed -= GenerateDataCard;
        if (scanAction != null) scanAction.action.performed -= EseguiScansione;
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.onChangeHandMassTool.RemoveListener(OnChangeHand);
            GameEvents.Instance.onDropMassTool.RemoveListener(OnDrop);
        }
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

                displayRisultato.text = massaMemorizzata+"kg";
                Debug.Log("Massa scansionata: " + massaMemorizzata + " kg");
            }
        }
    }

    private void GenerateDataCard(InputAction.CallbackContext context) 
    { 
        if (!context.performed) return;
        if (spawnCard == null) return;
        if(massaMemorizzata <= 0f) return;

        spawnCard.ValueReceiver(massaMemorizzata);
        spawnCard.SpawnDataCard();

    }

 
    private void ImpostaColoreLaser(Color colore)
    {
        lineRenderer.startColor = colore;
        lineRenderer.endColor = colore;
    }
}
