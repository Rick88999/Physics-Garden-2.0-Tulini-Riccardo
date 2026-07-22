using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DetectHandOnGrab))]
public abstract class VRToolOneHand : MonoBehaviour
{
    protected DetectHandOnGrab detectHandOnGrab;
    
    protected InputActionReference primaryAction;
    protected InputActionReference secondaryAction;

    protected virtual void Awake()
    {
        detectHandOnGrab = GetComponent<DetectHandOnGrab>();
        if (detectHandOnGrab == null)
        {
            Debug.LogError($"[VRTool] DetectHandOnGrab non trovato su {gameObject.name}!");
        }
    }

    protected virtual void OnEnable()
    {
        RebindActions();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeActions();
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public virtual void RebindActions()
    {
        UnsubscribeActions();

        if (detectHandOnGrab != null)
        {
            primaryAction = detectHandOnGrab.GetPrimaryAction();
            secondaryAction = detectHandOnGrab.GetSecondaryAction();
        }

        SubscribeActions();
    }

    protected virtual void SubscribeActions() { }
    protected virtual void UnsubscribeActions() { }
    protected virtual void UnsubscribeEvents() { }

    protected virtual void OnChangeHand(string newHand, int idRiser){ }
    protected virtual void OnDrop(int idRiser) { }
}
