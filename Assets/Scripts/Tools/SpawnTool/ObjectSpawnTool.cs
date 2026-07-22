using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectSpawnTool : VRToolOneHand
{
    private enum Forma { Cubo, Sfera, Cilindro }
    private enum Materiale { Legno, Roccia, Gomma, Metallo, Vetro }

    [Header("Configurazione Oggetto")]
    [SerializeField] private GameObject menuPrefab;
    [SerializeField] private Transform spawnAttach;
    [SerializeField] private Forma forma = Forma.Cubo;
    [SerializeField] private Materiale materiale = Materiale.Legno;

    [Header("Massa")]
    [SerializeField] private float massa = 1f;
    [SerializeField] private TextMeshProUGUI massaText;
    [SerializeField] private Slider massaSlider;

    [Header("Raggio")]
    [SerializeField] private float raggio = 1f;
    [SerializeField] private TextMeshProUGUI raggioText;
    [SerializeField] private Slider raggioSlider;

    [Header("Materiali")]
    [SerializeField] private List<Material> materiali = new List<Material>();
    [SerializeField] private float fattoreDiScala = 0.01f;

    private Dictionary<Materiale, Material> materialeMap = new Dictionary<Materiale, Material>();

    protected override void Awake()
    {
        base.Awake();

        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.onChangeHandSpawnerTool.AddListener(OnChangeHand);
            GameEvents.Instance.onDropSpawnerTool.AddListener(OnDrop);
        }

        if (materiali.Count >= 5)
        {
            materialeMap[Materiale.Legno] = materiali[0];
            materialeMap[Materiale.Roccia] = materiali[1];
            materialeMap[Materiale.Gomma] = materiali[2];
            materialeMap[Materiale.Metallo] = materiali[3];
            materialeMap[Materiale.Vetro] = materiali[4];
        }
        else
        {
            Debug.LogError("Assicurati di assegnare almeno 5 materiali alla lista 'materiali' nel Inspector.");
        }
    }

    protected override void SubscribeActions()
    {
        if (isActiveAndEnabled)
        {
            if (primaryAction != null) primaryAction.action.performed += OnSpawnPerformed;
            if (secondaryAction != null) secondaryAction.action.performed += OnOpenMenuPerformed;
        }
    }

    protected override void UnsubscribeActions()
    {
        if (primaryAction != null) primaryAction.action.performed -= OnSpawnPerformed;
        if (secondaryAction != null) secondaryAction.action.performed -= OnOpenMenuPerformed;
    }

    protected override void UnsubscribeEvents()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.onChangeHandSpawnerTool.RemoveListener(OnChangeHand);
            GameEvents.Instance.onDropSpawnerTool.RemoveListener(OnDrop);
        }
    }

    protected override void OnChangeHand(string newHand, int idRiser)
    {
        if (gameObject.GetInstanceID() == idRiser)
        {
            RebindActions();
            ChangeScreenPosition(newHand);
        }
    }

    protected override void OnDrop(int idRiser)
    {
        if (gameObject.GetInstanceID() == idRiser)
        {
            RebindActions();
            if (menuPrefab != null) menuPrefab.SetActive(false);
        }
    }

    private void OnSpawnPerformed(InputAction.CallbackContext context) => Spawn();
    private void OnOpenMenuPerformed(InputAction.CallbackContext context) => OpenMenu();

    private void OpenMenu()
    {
        if (menuPrefab != null)
            menuPrefab.SetActive(!menuPrefab.activeSelf);
    }

    private void Spawn()
    {
        GameObject oggetto = null;
        switch (forma)
        {
            case Forma.Cubo:
                oggetto = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case Forma.Sfera:
                oggetto = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                break;
            case Forma.Cilindro:
                oggetto = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                break;
        }

        if (oggetto != null)
        {
            Rigidbody rb = oggetto.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            oggetto.AddComponent<XRGrabInteractable>();
            
            if (spawnAttach != null) oggetto.transform.position = spawnAttach.position;
            oggetto.tag = "Spawned Object";

            float sizeM = raggio * fattoreDiScala;

            if (forma == Forma.Cilindro)
                oggetto.transform.localScale = new Vector3(sizeM, sizeM / 2f, sizeM);
            else
                oggetto.transform.localScale = Vector3.one * sizeM;

            rb.mass = massa;
            if (materialeMap.TryGetValue(materiale, out Material mat))
            {
                oggetto.GetComponent<Renderer>().material = mat;
            }
            else
            {
                Debug.LogError("Materiale non trovato nella mappa. Assicurati di aver assegnato i materiali correttamente.");
            }
        }
    }

    private void ChangeShape(Forma nuovaForma) => forma = nuovaForma;
    private void ChangeMaterial(Materiale nuovoMateriale) => materiale = nuovoMateriale;

    private void IncreaseMass(float amount)
    {
        massa += amount;
        if (massa > 100f) massa = 100f;
        if (massaText != null) massaText.text = massa.ToString("F1");
    }

    private void DecreaseMass(float amount)
    {
        if (massa > 1f)
        {
            massa -= amount;
            if (massa < 1f) massa = 1f;
            if (massaText != null) massaText.text = massa.ToString("F1");
        }
    }

    public void IncreaseMassOneOnTen() => IncreaseMass(0.1f);
    public void DecreaseMassOneOnTen() => DecreaseMass(0.1f);
    public void IncreaseMassOne() => IncreaseMass(1f);
    public void DecreaseMassOne() => DecreaseMass(1f);
    public void IncreaseMassTen() => IncreaseMass(10f);
    public void DecreaseMassTen() => DecreaseMass(10f);
    public void IncreaseMassHalf() => IncreaseMass(0.5f);
    public void DecreaseMassHalf() => DecreaseMass(0.5f);

    public void ChangeRadius()
    {
        if (raggioSlider == null) return;
        raggio = raggioSlider.value;  
        if (raggioText != null) raggioText.text = raggio.ToString();
    }

    private void ChangeScreenPosition(string newHand)
    {
        if (menuPrefab != null)
            menuPrefab.transform.localPosition = newHand == "Right" ? new Vector3(0.0500001907f, 0.00740000606f, 0.25f) : new Vector3(0.0500001907f, 0.00740000606f, -0.25f);
    }

    public void ChangeShapeCube() => ChangeShape(Forma.Cubo);
    public void ChangeShapeSphere() => ChangeShape(Forma.Sfera);
    public void ChangeShapeCylinder() => ChangeShape(Forma.Cilindro);

    public void ChangeMaterialWood() => ChangeMaterial(Materiale.Legno);
    public void ChangeMaterialRock() => ChangeMaterial(Materiale.Roccia);
    public void ChangeMaterialRubber() => ChangeMaterial(Materiale.Gomma);
    public void ChangeMaterialMetal() => ChangeMaterial(Materiale.Metallo);
    public void ChangeMaterialGlass() => ChangeMaterial(Materiale.Vetro);
}