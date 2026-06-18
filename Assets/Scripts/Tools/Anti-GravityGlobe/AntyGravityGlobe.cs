using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class AntyGravityGlobe : MonoBehaviour
{
    [SerializeField] private InputActionReference pushUpAction;
    [SerializeField] private float maxUpSpeed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private CharacterController controller;
    private bool grab;

    private float v = 0f;

    private XRGrabInteractable grabInteractable;
    private bool pushingUp;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);

        if (pushUpAction != null)
        {
            pushUpAction.action.performed += OnPushUp;
            pushUpAction.action.canceled += OnPushUpCanceled;
        }
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);

        if (pushUpAction != null)
        {
            pushUpAction.action.performed -= OnPushUp;
            pushUpAction.action.canceled -= OnPushUpCanceled;
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args) => grab = true;

    private void OnSelectExited(SelectExitEventArgs args)
    {
        pushingUp = false;
        grab = false;
    }

    private void OnPushUp(InputAction.CallbackContext ctx)
    {
        if (!grabInteractable.isSelected) return;

        pushingUp = true;
 
    }

    private void OnPushUpCanceled(InputAction.CallbackContext ctx)
    {
        pushingUp = false;
    }

    private void LateUpdate()
    {
        
        if (v < maxUpSpeed && pushingUp)
        {
            v += acceleration * Time.deltaTime;
            controller.Move(v * Time.deltaTime * Vector3.up);
            Debug.Log($"1 Accelerating up: {v}");
        }
        else if (v >= maxUpSpeed && pushingUp)
        {
            v = maxUpSpeed;
            controller.Move(v * Time.deltaTime * Vector3.up);
            Debug.Log($"2 Max speed reached: {v}");
        }
        else if (v > 0 && !pushingUp)
        {
            v -= Physics.gravity.y * Time.deltaTime;
            controller.Move(v * Time.deltaTime * Vector3.down);
            Debug.Log($"3 Decelerating: {v}");
        }
        else if (controller.isGrounded)
        {
            v = 0;
            if (grab) Debug.Log($"4 Stopped: {v}");
        }          
    }
}
