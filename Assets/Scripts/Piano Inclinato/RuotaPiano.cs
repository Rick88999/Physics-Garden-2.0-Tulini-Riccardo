using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSimpleInteractable))]
public class RuotaPiano : MonoBehaviour
{
    [Header("Puntatore al testo dell'angolo")]
    [SerializeField] private TextMeshProUGUI angoloText;

    [Header("Rampa")]
    [SerializeField] private Transform piano;
    [SerializeField] private float minAngle = 10f;
    [SerializeField] private float maxAngle = 60f;
    [SerializeField] private float rotazioneMax = 60f;

    [Header("Fattore di Smoothing")]
    [SerializeField] private float smoothingFactor = 0.15f; // ATTENZIONE: impostare tra 0.1 e 0.3
    [SerializeField] private float deadzone = 0.5f; 

    private XRSimpleInteractable interactXR;
    private IXRSelectInteractor currentInteractXR;
    private float angoloMano;
    private float angoloManopolaAttuale = 0f;
    private Vector3 fixedPosition;
    [SerializeField] private GameObject DataCardSpawner;
    private int actualPianoAngle = 0;

    void Awake()
    {
        interactXR = GetComponent<XRSimpleInteractable>();
        interactXR.selectEntered.AddListener(OnGrab);
        interactXR.selectExited.AddListener(OnRelease);
    }

    void Start()
    {
        fixedPosition = transform.position;
    }

    void LateUpdate()
    {
        
        transform.position = fixedPosition;

        
        if (currentInteractXR != null)
        {
            UpdateKnob(currentInteractXR.transform);
            TransmitResult();
        }
            
    }

    private void TransmitResult() { 
        if (DataCardSpawner != null) {
            DataCardSpawner.GetComponent<SpawnCard>().ValueReceiver(actualPianoAngle);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractXR = args.interactorObject;
        angoloMano = GetHandAngle(currentInteractXR.transform);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        currentInteractXR = null;
    }

    private void UpdateKnob(Transform hand)
    {
        float handAngle = GetHandAngle(hand);
        float delta = Mathf.DeltaAngle(angoloMano, handAngle);

        
        if (Mathf.Abs(delta) < deadzone)
            delta = 0f;

        angoloMano = handAngle;

        
        float smoothDelta = Mathf.Lerp(0f, delta, smoothingFactor);
        angoloManopolaAttuale = Mathf.Clamp(angoloManopolaAttuale + smoothDelta, 0f, rotazioneMax);
        transform.localRotation = Quaternion.Euler(-angoloManopolaAttuale, 0f, 0f);

        float t = angoloManopolaAttuale / rotazioneMax;
        float pianoAngle = Mathf.Lerp(minAngle, maxAngle, t);
        
        piano.localRotation = Quaternion.Euler(-pianoAngle, 0f, 0f);
        actualPianoAngle = (int)Mathf.Round(pianoAngle);
        angoloText.text = $"{actualPianoAngle}°";
    }

    private float GetHandAngle(Transform hand)
    {
        Vector3 dir = hand.position - transform.position;
        dir.x = 0f;
        return Mathf.Atan2(dir.y, dir.z) * Mathf.Rad2Deg;
    }

    public float GetCurrentPianoAngle()
    {
        return actualPianoAngle;
    }
}
