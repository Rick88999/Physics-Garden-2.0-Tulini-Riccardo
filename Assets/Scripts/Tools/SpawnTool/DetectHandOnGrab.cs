using System.Buffers;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEngine.Rendering.GPUSort;

[RequireComponent(typeof(XRGrabInteractable))]
public class DetectHandOnGrab : MonoBehaviour
{
    private XRGrabInteractable grab;
    [SerializeField] private Transform attachRight;
    [SerializeField] private Transform attachLeft;
    [SerializeField] private InputActionReference scanActionRight;
    [SerializeField] private InputActionReference scanActionLeft;
    [SerializeField] private InputActionReference spawnActionRight;
    [SerializeField] private InputActionReference spawnActionLeft;
    private InputActionReference scanAction;
    private InputActionReference spawnAction;
    private bool? isRight = null;
    private bool? beforeGrabIsRight = null;
    private int counterDebug = 0;


    [SerializeField] private XRGrabInteractable grabbable;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerHand")) return;
#if UNITY_EDITOR && DEBUG
        if (counterDebug == 0)
        {
            counterDebug++;
            return;
        }
#endif
        
        beforeGrabIsRight = isRight;
        var handedness = other.GetComponentInParent<ManoEco>();
        if (handedness == null)
        {
            Debug.LogWarning("Interactor senza Handedness nei parent: non posso determinare la mano.");
            return;
        }
        

        isRight = handedness.hand == Hand.Right;
        grabbable.attachTransform = (bool)isRight ? attachRight : attachLeft;
        Debug.Log($"TriggerEnter con mano: {handedness.hand}; AttachTransform: {grabbable.attachTransform.name}");

    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerHand")) return;

        if (beforeGrabIsRight == null)
        {
            isRight = null;
            grabbable.attachTransform = null;
        }
        else if (beforeGrabIsRight != null)
        {
            isRight = beforeGrabIsRight;
            grabbable.attachTransform = (bool)isRight ? attachRight : attachLeft;
        }
    }



    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnSelectEntered);
        grab.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // args.interactorObject è un IXRSelectInteractor
        // Per risalire al GameObject: cast a Component (gli interactor in XRI sono MonoBehaviour)
        var interactorComponent = args.interactorObject as Component;
        if (interactorComponent == null)
            return;

        var handedness = interactorComponent.GetComponentInParent<ManoEco>();
        if (handedness == null)
        {
            Debug.LogWarning("Interactor senza Handedness nei parent: non posso determinare la mano.");
            return;
        }

        
        scanAction = (handedness.hand == Hand.Right) ? scanActionRight : scanActionLeft;
        spawnAction = (handedness.hand == Hand.Right) ? spawnActionRight : spawnActionLeft;
        if (scanAction == null || spawnAction == null)
        {
            Debug.LogWarning("ScanAction o SpawnAction non assegnate per la mano: " + handedness.hand);
            return;
        }
        else
            Debug.Log($"Grab con mano: {handedness.hand}; ScanAction: {scanAction.name}; SpawnAction: {spawnAction.name}");
        
        grab.attachEaseInTime = 0f;

    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // opzionale
    }

    public InputActionReference GetScanAction()
    {
        return scanAction;
    }

    public InputActionReference GetSpawnAction()
    {
        return spawnAction;
    }
}