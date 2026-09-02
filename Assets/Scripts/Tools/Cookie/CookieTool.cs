using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CookieTool : VRToolOneHand
{
    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cookieCrunchSound;

    protected override void Awake()
    {
        base.Awake();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.onChangeHandTeleportTool.AddListener(OnChangeHand);
            GameEvents.Instance.onDropTeleportTool.AddListener(OnDrop);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        RebindActions();
    }

    private void OnReleased(SelectExitEventArgs args) => isHeld = false;


    protected override void SubscribeActions()
    {
        if (isActiveAndEnabled && primaryAction != null)
        {
            primaryAction.action.performed += OnInputPerformed;
            primaryAction.action.Enable();
        }
    }

    protected override void UnsubscribeActions()
    {
        if (primaryAction != null && primaryAction.action != null)
            primaryAction.action.performed -= OnInputPerformed;
    }

    protected override void UnsubscribeEvents()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.onChangeHandTeleportTool.RemoveListener(OnChangeHand);
            GameEvents.Instance.onDropTeleportTool.RemoveListener(OnDrop);
        }
    }

    protected override void OnChangeHand(string newHand, int idRiser)
    {
        if (gameObject.GetInstanceID() == idRiser) RebindActions();
    }

    protected override void OnDrop(int idRiser)
    {
        if (gameObject.GetInstanceID() == idRiser) RebindActions();
    }

    private void OnInputPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isHeld)
        {
            EatCookie();
        }
    }

    private void EatCookie()
    {
        Debug.Log("Cookie eaten!");
        audioSource.PlayOneShot(cookieCrunchSound);
        Destroy(gameObject);
    }
}
