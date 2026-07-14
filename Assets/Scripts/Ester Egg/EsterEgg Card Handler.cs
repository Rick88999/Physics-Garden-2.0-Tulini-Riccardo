using UnityEngine;

public class EsterEggCardHandler : MonoBehaviour
{
    [System.Serializable]
    private struct CardData
    {
        public GameObject cardPrefab;
        public EsterEggCardType type;
        public Transform spawnPoint;
    }

    [SerializeField] private CardData[] cardDataArray;

    public void SpawnCard(EsterEggCardType cardType)
    {
        CardData cardData = System.Array.Find(cardDataArray, data => data.type == cardType);
        if (cardData.cardPrefab != null && cardData.spawnPoint != null)
        {
            GameObject spawned = Instantiate(cardData.cardPrefab, cardData.spawnPoint.position, cardData.spawnPoint.rotation, transform);
            spawned.SetActive(true);
        }
        else
        {
            Debug.LogError("EsterEggCardHandler: Card prefab or spawn point is not assigned.");
        }
    }

}
