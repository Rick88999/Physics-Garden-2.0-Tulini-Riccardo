using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private List<GameObject> menuPages = new List<GameObject>();
    [SerializeField] private List<bool> states = new List<bool>();
    [SerializeField] private bool turnOnStartMenu = false;
    [SerializeField] private InputActionReference toggleMenuAction;


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
        if (turnOnStartMenu)
        {
            CloseStillActive();
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
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
