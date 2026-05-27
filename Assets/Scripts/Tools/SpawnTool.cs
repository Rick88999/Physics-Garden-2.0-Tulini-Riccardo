using UnityEngine;

public class SpawnTool : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Transform spawnPoint;

    public void Spawn()
    {
        if (prefabToSpawn != null && spawnPoint != null)
        {
            GameObject newTool = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            newTool.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Prefab or Spawn Point is not assigned.");
        }
    }
}
