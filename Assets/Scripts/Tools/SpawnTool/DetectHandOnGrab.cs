using System.Buffers;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEngine.Rendering.GPUSort;

[RequireComponent(typeof(XRGrabInteractable))]
public class DetectHandOnGrab : VRGrabDetector
{
    [Header("Controls Settings")]
    [SerializeField] private TipoTool tipoTool;
    [SerializeField] private Transform attachRight;
    [SerializeField] private Transform attachLeft;
    [SerializeField] private InputActionReference primaryActionRight;
    [SerializeField] private InputActionReference primaryActionLeft;
    [SerializeField] private InputActionReference secondaryActionRight;
    [SerializeField] private InputActionReference secondaryActionLeft;
    [Header("Debug Values")]
    [SerializeField] private Transform alreadyAttach;
    [SerializeField] private InputActionReference primaryAction; //Dorsale
    [SerializeField] private InputActionReference secondaryAction;//Pulsante
    private bool? isRight = null;
    private bool? beforeGrabIsRight = null;

    protected override void Awake()
    {
        base.Awake();
        grabbable.selectEntered.AddListener(OnSelectEntered);
        grabbable.selectExited.AddListener(OnSelectExited);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerHand") || (attachLeft == null && attachRight == null)) return;

        beforeGrabIsRight = isRight;
        ManoEco handedness = other.GetComponentInParent<ManoEco>();
        if (handedness == null)
        {
            Debug.LogWarning("Interactor senza Handedness nei parent: non posso determinare la mano.");
            return;
        }
        

        isRight = handedness.hand == Hand.Right;
        grabbable.attachTransform = (bool)isRight ? attachRight : attachLeft;
        //Debug.Log($"TriggerEnter con mano: {handedness.hand}; AttachTransform: {grabbable.attachTransform.name}");

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerHand") || (attachLeft == null && attachRight == null)) return;

        if (beforeGrabIsRight == null)
        {
            isRight = null;
            grabbable.attachTransform = null;
        }
        else if (beforeGrabIsRight != null)
        {
            isRight = beforeGrabIsRight;
            grabbable.attachTransform = (bool)isRight ? attachRight : attachLeft;
            alreadyAttach = grabbable.attachTransform;
        }
        //Debug.Log($"Mano uscita: {isRight} con AttachTransform: {grabbable.attachTransform?.name}");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        var interactorComponent = args.interactorObject as Component;
        if (interactorComponent == null)
            return;

        ManoEco handedness = interactorComponent.GetComponentInParent<ManoEco>();
        if (handedness == null)
        {
            Debug.LogWarning("Interactor senza Handedness nei parent: non posso determinare la mano.");
            return;
        }
        if(primaryActionRight != null && primaryActionLeft != null)
            primaryAction = (handedness.hand == Hand.Right) ? primaryActionRight : primaryActionLeft;

        if(secondaryActionRight != null && secondaryActionLeft != null)
            secondaryAction = (handedness.hand == Hand.Right) ? secondaryActionRight : secondaryActionLeft;

        switch(tipoTool)
        {
            case TipoTool.Spawner:
                GameEvents.Instance.onChangeHandSpawnerTool.Invoke(handedness.hand.ToString(), gameObject.GetInstanceID());
                break;
            case TipoTool.MassSensor:
                GameEvents.Instance.onChangeHandMassTool.Invoke(handedness.hand.ToString(), gameObject.GetInstanceID());
                break;
            case TipoTool.Teleport:
                GameEvents.Instance.onChangeHandTeleportTool.Invoke(handedness.hand.ToString(), gameObject.GetInstanceID());
                break;
        }

    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        primaryAction = null;   secondaryAction = null;
        switch(tipoTool)
        {
            case TipoTool.Spawner:
                GameEvents.Instance.onDropSpawnerTool.Invoke(gameObject.GetInstanceID());
                break;
            case TipoTool.MassSensor:
                GameEvents.Instance.onDropMassTool.Invoke(gameObject.GetInstanceID());
                break;
            case TipoTool.Teleport:
                GameEvents.Instance.onDropTeleportTool.Invoke(gameObject.GetInstanceID());
                break;
        }
    }

    public InputActionReference GetPrimaryAction()
    {
        return primaryAction;
    }

    public InputActionReference GetSecondaryAction()
    {
        return secondaryAction;
    }
}