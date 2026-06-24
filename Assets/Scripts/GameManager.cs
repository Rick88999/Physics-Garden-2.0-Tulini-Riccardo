using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public struct SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(Vector3 v) { x = v.x; y = v.y; z = v.z; }
    public Vector3 ToVector3() => new Vector3(x, y, z);
}

public struct SerializableQuaternion
{
    public float x, y, z, w;
    public SerializableQuaternion(Quaternion q) { x = q.x; y = q.y; z = q.z; w = q.w; }
    public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
}

public struct SaveInventoryButton
{
    public int index;
    public bool state;
}

public struct SavedStep
{
    public int id_step;
    public string state;
}
public struct SavedQuest
{
    public int id_quest;
    public string state;
    public SavedStep[] allSteps;
}
public struct SavedState
{
    public int id_player;
    public string name_player;
    public SerializableVector3 position_player;      
    public SerializableQuaternion rotation_player;   
    public SerializableVector3 position_companion;   
    public SerializableQuaternion rotation_companion;
    public SavedQuest[] allQuests;
    public SaveInventoryButton[] allFormulaButtons;
    public SaveInventoryButton[] allConstantButtons;
    public SaveInventoryButton[] allToolsButtons;
}

public class GameManager : MonoBehaviour
{
    [Header("Quest Manager")]
    [SerializeField] private QuestManager questManager;

    [Header("Player & Companion Bot & StartTower")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject companion;
    [SerializeField] private GameObject startTower;

    [Header("Base Player Posizione & Rotazione")]
    [SerializeField] private Vector3 basePlayerPosition;
    [SerializeField] private Quaternion basePlayerRotation;

    [Header("Menu Manager")]
    [SerializeField] private PlayerMenuManager playerMenuManager;
    [SerializeField] private GravityGunController gravityGunController;

    [Header("Inventory")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Player Audio")]
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip playerAudioClip;

    [Header("Setting Light & Audio")]
    [SerializeField] private SettingsController settingsController;

    private SavedState actualSavedState;
    private Language actualLanguage = Language.Italian;

    private void ResetLight() => settingsController.SetSliderSettings();
    
    public void SetLanguage(Language language) => actualLanguage = language;
    public Language GetLanguage() => actualLanguage;

    public void NewSaveGame(string name)
    {
        int nextId = 0;
        SavedState savedState = new SavedState();
        if (Directory.Exists(Application.persistentDataPath))
        {
            string[] fileSalvati = Directory.GetFiles(Application.persistentDataPath, "*.json");
            nextId = fileSalvati.Length;
        }

        savedState.id_player = nextId;
        savedState.name_player = name;
        savedState.position_player = new SerializableVector3(basePlayerPosition);
        savedState.rotation_player = new SerializableQuaternion(basePlayerRotation);
        savedState.position_companion = new SerializableVector3(companion.transform.position);
        savedState.rotation_companion = new SerializableQuaternion(companion.transform.rotation);
        actualSavedState = savedState;
        ResetLight();
        playerMenuManager.UnlockMenu();
        //inventoryManager.gameObject.SetActive(true);
        inventoryManager.AwakeButtons();
        //inventoryManager.gameObject.SetActive(false);
        player.transform.position = basePlayerPosition;
        player.transform.rotation = basePlayerRotation;
        startTower.SetActive(false);
        AudioChangeClip();
    }

    public void SaveGame()
    {
        actualSavedState.position_player = new SerializableVector3(player.transform.position);
        actualSavedState.rotation_player = new SerializableQuaternion(player.transform.rotation);

        actualSavedState.position_companion = new SerializableVector3(companion.transform.position);
        actualSavedState.rotation_companion = new SerializableQuaternion(companion.transform.rotation);
        actualSavedState.allQuests = questManager.getSavedQuests();
        SaveInventory();
        string json = JsonConvert.SerializeObject(actualSavedState, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, $"{actualSavedState.name_player}.json");
        File.WriteAllText(path, json);
        Debug.Log("Dati salvati in: " + path);
    }

    public void SaveAndClose()
    {
        SaveGame();
        Close();
    }

    public void Close()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Editor + Debug in Editor
        #else
            Application.Quit();
        #endif
    }


    public void LoadSavedGame(string name)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{name}.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedState savedState = JsonConvert.DeserializeObject<SavedState>(json);
            questManager.settingsSaves(savedState.allQuests);
            player.transform.position = savedState.position_player.ToVector3();
            player.transform.rotation = savedState.rotation_player.ToQuaternion();
            companion.transform.position = savedState.position_companion.ToVector3();
            companion.transform.rotation = savedState.rotation_companion.ToQuaternion();
            actualSavedState = savedState;
            ResetLight();
            AudioChangeClip();
            BotController botController = companion.GetComponent<BotController>();
            botController.StartFollow();
            LoadInvetory();
            startTower.SetActive(false);
            playerMenuManager.UnlockMenu();
            foreach (SavedQuest quest in actualSavedState.allQuests)
            {
                if(quest.id_quest == 0)
                {
                    if (quest.state == "Completed" || quest.state == "OnGoing")
                    {
                        foreach (SavedStep step in quest.allSteps)
                        {
                            if (step.id_step == 2)
                            {
                                if (step.state == "Completed")
                                    gravityGunController.Activation();
                                
                            }

                            if (step.id_step == 5)
                            {
                                if (step.state == "Completed")
                                {
                                    inventoryManager.EnableInventory();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void DeleteSavedGame(string name)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{name}.json");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Salvataggio eliminato: " + path);
        }
    }

    public string[] GetSavedFiles()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            string[] fileSalvati = Directory.GetFiles(Application.persistentDataPath, "*.json");
            return fileSalvati;
        }
        return new string[0];
    }

    private void SaveInventory()
    {
        actualSavedState.allFormulaButtons = inventoryManager.SaveFormulaButtons();
        actualSavedState.allConstantButtons = inventoryManager.SaveConstantButtons();
        actualSavedState.allToolsButtons = inventoryManager.SaveToolsButtons();
    }

    private void LoadInvetory()
    {
        inventoryManager.SetActiveFormulaButtons(actualSavedState.allFormulaButtons);
        inventoryManager.SetActiveConstantButtons(actualSavedState.allConstantButtons);
        inventoryManager.SetActiveToolsButtons(actualSavedState.allToolsButtons);
    }

    private void AudioChangeClip()
        {
        playerAudioSource.Stop();
        playerAudioSource.clip = playerAudioClip;
        playerAudioSource.Play();
    }
}
