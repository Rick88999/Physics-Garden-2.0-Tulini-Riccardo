/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandMenu : MonoBehaviour
{
    
    public GameObject radialMenu;
    public Transform Player;
    public float spawnDistance = 1.0f;
    public InputActionReference toggleMenuAction;
    public GameObject Base;
    [SerializeField] private List<GameObject> Modalita = new List<GameObject>();
    [SerializeField] public bool enableButton = false;

    public void updateButton() 
    {         
        enableButton = true;
    }
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
        //if (Time.time - lastInputTimer > timerResetInput) return;
        //lastInputTimer = Time.time;
        if (enableButton) 
        { 
            if (Base != null && !Base.activeSelf)
            {
                ResetHand();
                return;
            }
            bool isMenuActive = !radialMenu.activeSelf;
            radialMenu.SetActive(isMenuActive);

            if (isMenuActive && Player != null)
            {
                Vector3 spawnPosition = Player.position + Player.forward * spawnDistance;

                radialMenu.transform.position = spawnPosition;
            }
        }
        
    }

    private void ResetHand()
    {
        foreach (GameObject modalita in Modalita)
        {
            if (modalita != Base)
            {
                modalita.SetActive(false);
            }
        }
        Base.SetActive(true);
    }
}*/