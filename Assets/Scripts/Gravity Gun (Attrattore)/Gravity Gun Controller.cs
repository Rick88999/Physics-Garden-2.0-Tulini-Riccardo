using UnityEngine;
using UnityEngine.InputSystem;

public class GravityGunController : MonoBehaviour
{
    [SerializeField] private InputActionReference rightActivator;
    [SerializeField] private InputActionReference leftActivator;
    [SerializeField] private GameObject[] pointers = new GameObject[2];

    [SerializeField] private GameObject[] lineObjects = new GameObject[2];

    private bool stopInMenu = true;

    private void OnEnable()
    {
        
        rightActivator.action.performed += ToggleLineRenderer;
        leftActivator.action.performed += ToggleLineRenderer;
    }

    private void OnDisable()
    {
        
        rightActivator.action.performed -= ToggleLineRenderer;
        leftActivator.action.performed -= ToggleLineRenderer;
    }

    private void ToggleLineRenderer(InputAction.CallbackContext context)
    {
        if (stopInMenu) return;
        if (context.action == rightActivator.action)
        {
            pointers[0].SetActive(!pointers[0].activeSelf);
            lineObjects[0].SetActive(!lineObjects[0].activeSelf);
        }
        else if (context.action == leftActivator.action)
        {
            pointers[1].SetActive(!pointers[1].activeSelf);
            lineObjects[1].SetActive(!lineObjects[1].activeSelf);
        }
    }

    public void Activation() => stopInMenu = false;

    
}
