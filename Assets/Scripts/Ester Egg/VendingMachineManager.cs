using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VendingMachineManager : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socketInteractor;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject cookiePrefab;
    [SerializeField] private AudioClip soundSpawn;
    [SerializeField] private AudioClip soundError;
    [SerializeField] private AudioClip soundCoinInserted;

    private AudioSource audioSource;
    private bool coinInserted = false;

    private void Awake() => audioSource = GetComponent<AudioSource>();

    public void InsertCoin()
    {
        if (socketInteractor != null)
        {
            IXRSelectInteractable interactable = socketInteractor.GetOldestInteractableSelected();
            if (interactable != null)
            {
                Destroy(interactable.transform.gameObject);
                coinInserted = true;
                audioSource.PlayOneShot(soundCoinInserted);
            }
        }
    }

    public void SpawnCookie()
    {
        if (cookiePrefab != null && spawnPoint != null && coinInserted)
        {
            GameObject spawnedCookie = Instantiate(cookiePrefab, spawnPoint.position, spawnPoint.rotation);
            audioSource.PlayOneShot(soundSpawn);
            coinInserted = false;
        }
        else audioSource.PlayOneShot(soundError);
    }
}
