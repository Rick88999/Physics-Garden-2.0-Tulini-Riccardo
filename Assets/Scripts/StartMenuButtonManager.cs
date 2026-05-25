using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard;

public class GestioneMenuSalvataggi : MonoBehaviour
{
    [Header("Configurazione UI")]
    [SerializeField]private GameManager gameManager;
    [SerializeField]private Transform panelContenitore; 
    [SerializeField]private GameObject prefabBottone;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] public XRKeyboard xrKeyboard;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSoundError;

    private void Start()
    {
        inputField.onSelect.AddListener(OnInputSelected);
    }

    private void OnInputSelected(string value)
    {
        xrKeyboard.gameObject.SetActive(true);
        xrKeyboard.Open(inputField);
    }

    private void Update()
    {

        if (xrKeyboard != null && xrKeyboard.gameObject.activeSelf)
        {
            inputField.text = xrKeyboard.text;
        }
    }
    public void ControlNewGame(string name)
    {
        string[] saveFiles = gameManager.GetSavedFiles();
        if (saveFiles.Length > 0)
        {
            audioSource.PlayOneShot(clickSoundError);
        }
        else
        {
            xrKeyboard.gameObject.SetActive(false);
            gameManager.NewSaveGame(name);
        }
    }

    public void GenerateButton()
    {
        string[] saveFiles = gameManager.GetSavedFiles();
        if (saveFiles.Length == 0)
        {
            Debug.Log("Nessun file di salvataggio trovato.");
            return;
        }
        else
        {
            foreach (string saveFile in saveFiles)
            {
                string nameFile = Path.GetFileNameWithoutExtension(saveFile);
                GameObject newButton = Instantiate(prefabBottone, panelContenitore);
                newButton.SetActive(true);
                TextMeshProUGUI testoBottone = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (testoBottone != null)
                {
                    testoBottone.text = nameFile;
                }

                Button button = newButton.GetComponent<Button>();
                if(button != null)
                {
                    button.onClick.AddListener(() => gameManager.LoadSavedGame(nameFile));
                }
            }
        }
    }

    public void OnStartButtonClick() =>ControlNewGame(inputField.text);
    public void OnKeybordSubmit() => ControlNewGame(inputField.text);



}
