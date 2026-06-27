using System;
using UnityEngine;

public class SpawnTool : MonoBehaviour
{
    [Header("Tool to spawn settings")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform spawnPoint;
    [Header("Audio Settings")]
    [SerializeField] private SettingsController[] settingsControllers;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnSound;

    public void Spawn()
    {
        if (prefabToSpawn != null && spawnPoint != null)
        {
            GameObject newTool = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            AudioSource audioSource = newTool.GetComponent<AudioSource>();
            if (audioSource != null && settingsControllers.Length > 0)
            {
                foreach (SettingsController settingsController in settingsControllers)
                {
                    settingsController.InsertNewToolInList(audioSource);
                }
            }

            newTool.SetActive(true);
            if (audioSource != null && spawnSound != null) audioSource.PlayOneShot(spawnSound);
            
            GameEvents.Instance.onToolSpawned.Invoke(newTool.name);
        }
        else
        {
            Debug.LogWarning("Prefab or Spawn Point is not assigned.");
        }
    }
}
