using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMenuManager : MonoBehaviour
{
    [Header("Menu Canvas")]
    [SerializeField] private GameObject menuCanvas;
    [Header("Menu Pages")]
    [SerializeField] private List<GameObject> menuPages = new List<GameObject>();
    [SerializeField] private List<bool> states = new List<bool>();
    [Header("Menu Settings")]
    [SerializeField] private bool turnOnStartMenu = false;
    [SerializeField] private InputActionReference toggleMenuAction;
    [Header("Menu Positioning")]
    [SerializeField] private Camera playerCamera; 
    [SerializeField] private float menuDistance = 1.5f;
    [SerializeField] private float menuHeight = 0f;


    private void OnEnable()
    {

        if (toggleMenuAction != null)
        {
            toggleMenuAction.action.performed += ToggleMenu;
        }
    }

    private void OnDisable()
    {

        if (toggleMenuAction != null)
        {
            toggleMenuAction.action.performed -= ToggleMenu;
        }
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        if (!turnOnStartMenu) return;

        CloseStillActive();

        bool willOpen = !menuCanvas.activeSelf;

        if (willOpen)
        {
            PlaceMenuInFrontOfPlayer();
        }

        menuCanvas.SetActive(willOpen);
    }

    private void PlaceMenuInFrontOfPlayer()
    {
        Transform cam = playerCamera.transform;

        Vector3 flatForward = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;

        Vector3 targetPosition = cam.position + flatForward * menuDistance;
        targetPosition.y += menuHeight;

        menuCanvas.transform.position = targetPosition;
        menuCanvas.transform.rotation = Quaternion.LookRotation(flatForward);
    }

    public void TogglePage(string pageName)
    {
        for (int i = 0; i < menuPages.Count; i++)
        {
            if (menuPages[i].name == pageName)
            {
                FindPage(i);
                return;
            }
        }
    }

    private void CloseStillActive()
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i])
            {
                deactivatePage(i);
                states[i] = false;
            }
        }
    }

    private void FindPage(int i)
    {
        if (states[i])
        {
            deactivatePage(i);
            states[i] = false;
        }
        else
        {
            activatePage(i);
            CloseStillActive();
            states[i] = true;
        }
    }
    private void activatePage(int i) => menuPages[i].SetActive(true);
    private void deactivatePage(int i) => menuPages[i].SetActive(false);

    public void UnlockMenu() => turnOnStartMenu = true;
}
