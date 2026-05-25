using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public struct SavedStep
{
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
    public UnityEngine.Vector3 position_player;
    public UnityEngine.Quaternion rotation_player;
    public UnityEngine.Vector3 position_companion;
    public UnityEngine.Quaternion rotation_companion;
    public SavedQuest[] allQuests; 
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject companion;
    [SerializeField] private SavedState actualSavedState;
    [SerializeField] private Vector3 basePlayerPosition;
    [SerializeField] private Quaternion basePlayerRotation;
    

    public void NewSaveGame(string name)
    {
        int nextId = 0;
        SavedState savedState = new SavedState();
        if (Directory.Exists(Application.persistentDataPath))
        {
            // Ottiene un array con i percorsi completi di tutti i file .json
            string[] fileSalvati = Directory.GetFiles(Application.persistentDataPath, "*.json");
            nextId = fileSalvati.Length;
        }

        savedState.id_player = nextId;
        savedState.name_player = name;
        savedState.position_player = basePlayerPosition;
        savedState.rotation_player = basePlayerRotation;
        savedState.position_companion = companion.transform.position;
        savedState.rotation_companion = companion.transform.rotation;
        actualSavedState = savedState;
        player.transform.position = basePlayerPosition;
        player.transform.rotation = basePlayerRotation;
    }

    public void SaveGame()
    {
        actualSavedState.position_player = player.transform.position;
        actualSavedState.rotation_player = player.transform.rotation;

        actualSavedState.position_companion = companion.transform.position;
        actualSavedState.rotation_companion = companion.transform.rotation;

        actualSavedState.allQuests = questManager.getSavedQuests();

        string json = JsonConvert.SerializeObject(actualSavedState, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, $"{actualSavedState.name_player}.json");
        File.WriteAllText(path, json);
        Debug.Log("Dati salvati in: " + path);
    }

    public void LoadSavedGame(string name)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{name}.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedState savedState = JsonConvert.DeserializeObject<SavedState>(json);
            questManager.settingsSaves(savedState.allQuests);
            player.transform.position = savedState.position_player;
            player.transform.rotation = savedState.rotation_player;
            companion.transform.position = savedState.position_companion;
            companion.transform.rotation = savedState.rotation_companion;
            actualSavedState = savedState;
            BotController botController = companion.GetComponent<BotController>();
            botController.StartFollow();
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
}
