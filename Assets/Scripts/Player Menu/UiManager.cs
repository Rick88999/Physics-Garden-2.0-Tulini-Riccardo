using System.Collections;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject lootUi;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lootSound;
    [Header("Menu Positioning")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float menuDistance = 1.5f;
    [SerializeField] private float menuHeight = 0f;

    private Language language;
    private void OnEnable()
    {
        GameEvents.Instance.onNewLootAcquired.AddListener(onLootAcquired);
    }

    private void OnDisable()
    {
        GameEvents.Instance.onNewLootAcquired.RemoveListener(onLootAcquired);
    }

    private void onLootAcquired(string lootName, TipoLoot tipoLoot)
    {
        StartCoroutine(ShowLootUI(lootName, tipoLoot));
    }

    private IEnumerator ShowLootUI(string lootName, TipoLoot tipoLoot)
    {
        if(gameManager.GetLanguage() == Language.English)
        {
            language = Language.English;
        }
        else if(gameManager.GetLanguage() == Language.Italian)
        {
            language = Language.Italian;
        }
        if (lootUi != null)
        {
            lootUi.SetActive(true);
            PlaceMenuInFrontOfPlayer();
            GameObject panel = lootUi.transform.GetChild(0).gameObject;
            TextMeshProUGUI lootText = panel.GetComponentInChildren<TextMeshProUGUI>();
            if (lootText != null)
            {
                if (tipoLoot == TipoLoot.Card)
                {
                    if (language == Language.English)
                    {
                        lootText.text = $"New card unlocked:\n {lootName}";
                    }
                    else if (language == Language.Italian)
                    {
                        lootText.text = $"Nuova carta sbloccata:\n {lootName}";
                    }
                }
                else if (tipoLoot == TipoLoot.Tool)
                {
                    if (language == Language.English)
                    {
                        lootText.text = $"New tool unlocked:\n {lootName}";
                    }
                    else if (language == Language.Italian)
                    {
                        lootText.text = $"Nuovo strumento sbloccato:\n {lootName}";
                    }
                }
                else if (tipoLoot == TipoLoot.Button)
                {
                    if (language == Language.English)
                    {
                        lootText.text = $"New button unlocked:\n {lootName}, check your inventory";
                    }
                    else if (language == Language.Italian)
                    {
                        lootText.text = $"Nuovo pulsante sbloccato:\n {lootName}, controlla l'inventario";
                    }
                }
                audioSource.PlayOneShot(lootSound);
            }
            yield return new WaitForSeconds(4f);
            lootUi.SetActive(false);
        }
    }

    private void PlaceMenuInFrontOfPlayer()
    {
        Transform cam = playerCamera.transform;

        Vector3 flatForward = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;

        Vector3 targetPosition = cam.position + flatForward * menuDistance;
        targetPosition.y += menuHeight;

        lootUi.transform.position = targetPosition;
        lootUi.transform.rotation = Quaternion.LookRotation(flatForward);
    }
}

