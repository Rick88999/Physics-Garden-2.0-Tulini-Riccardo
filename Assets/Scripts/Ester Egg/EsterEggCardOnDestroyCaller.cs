using UnityEngine;

public class EsterEggCardOnDestroyCaller : MonoBehaviour
{
    private EsterEggCardHandler cardHandler;
    [SerializeField] private EsterEggCardType cardTypeToSpawn;

    private static bool isQuitting;

    private void Start() => cardHandler = GetComponentInParent<EsterEggCardHandler>();

    private void Update()
    {
        Vector3 position = transform.position;

        if (position.y < -21f)
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit() => isQuitting = true;

    private void OnDestroy()
    {
        if (isQuitting || !gameObject.scene.isLoaded) return;

        if (cardHandler == null) cardHandler = GetComponentInParent<EsterEggCardHandler>();

        if (cardHandler != null)
        {
            cardHandler.SpawnCard(cardTypeToSpawn);
        }
        else
        {
            Debug.LogError("EsterEggCardOnDestroyCaller: Non è stato trovato EsterEggCardHandler nel genitore.");
        }
    }
}
