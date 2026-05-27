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
    [SerializeField]private GameObject prefabBottoneLoad;
    [SerializeField]private GameObject prefabBottoneDelete;
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
        bool nameExists = false;

        if (saveFiles.Length > 0)
        {
            foreach(string file in saveFiles)
            {
                string nameFile = Path.GetFileNameWithoutExtension(file);
                if (nameFile == name) nameExists = true;
            }
            if (nameExists)
            {
                audioSource.PlayOneShot(clickSoundError);
                return;
            }
            else
            {
                xrKeyboard.gameObject.SetActive(false);
                gameManager.NewSaveGame(name);
            }
        }
        else
        {
            xrKeyboard.gameObject.SetActive(false);
            gameManager.NewSaveGame(name);
        }

    }

    private void GenerateButton(string command)
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
                GameObject newButton = null;
                if(command == "Load") newButton = Instantiate(prefabBottoneLoad, panelContenitore);
                else if(command == "Delete") newButton = Instantiate(prefabBottoneDelete, panelContenitore);
                newButton.SetActive(true);
                TextMeshProUGUI testoBottone = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (testoBottone != null)
                {
                    testoBottone.text = nameFile;
                }

                Button button = newButton.GetComponent<Button>();
                if(button != null)
                {
                    if (command == "Load") button.onClick.AddListener(() => gameManager.LoadSavedGame(nameFile));
                    else if (command == "Delete") button.onClick.AddListener(() => gameManager.DeleteSavedGame(nameFile));
                }
            }
        }
    }

    public void ClearButtons()
    {
        foreach (Transform child in panelContenitore)
        {
            if(child != null && child.name != "Back") Destroy(child.gameObject);
        }
    }

    public void OnStartButtonClick() =>ControlNewGame(inputField.text);
    public void OnKeybordSubmit() => ControlNewGame(inputField.text);
    public void GenerateButtonLoad() => GenerateButton("Load");
    public void GenerateButtonDelete() => GenerateButton("Delete");



}
