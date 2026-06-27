using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMenuManager : MonoBehaviour
{
    [Header("Menu Canvas")]
    [SerializeField] private GameObject menuCanvas;
     [SerializeField] private GameObject saveNotificationPanel;
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
    [SerializeField] private float fadeDuration = 0.4f;
    [Header("Player Audio Source")]
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip saveDataSound;
    private CanvasGroup gruppoCanvas;

    private void Awake()
    {
        if (saveNotificationPanel != null)
            gruppoCanvas = saveNotificationPanel.GetComponent<CanvasGroup>();
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

    public IEnumerator ShowSaveNotification(float duration)
    {
        if (playerAudioSource != null && saveDataSound != null)
        {
            playerAudioSource.PlayOneShot(saveDataSound);
        }
        saveNotificationPanel.SetActive(true);

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            gruppoCanvas.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        gruppoCanvas.alpha = 1f;

        yield return new WaitForSeconds(duration);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            gruppoCanvas.alpha = Mathf.Clamp01(1f - t / fadeDuration);
            yield return null;
        }
        gruppoCanvas.alpha = 0f;

        saveNotificationPanel.SetActive(false);
    }
}
