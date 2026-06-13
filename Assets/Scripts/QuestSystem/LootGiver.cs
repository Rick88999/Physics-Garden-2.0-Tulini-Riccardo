using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootGiver : MonoBehaviour
{
    [Header("Loot Settings: Il loot nel componente deve essere tutto dello stesso tipo")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private string nameLootItalian;
    [SerializeField] private string nameLootEnglish;
    [SerializeField] private List<GameObject> lootPrefabs = new();
    [SerializeField] private Transform lootSpawnPoint;
    [SerializeField] private TipoLoot tipoLoot;
    [SerializeField] private bool main;
     
    private string nameLoot;
    public void GiveLoot()
    {
        if (lootPrefabs.Count == 0)
        {
            Debug.LogWarning("No loot prefabs assigned to the LootGiver.");
            return;
        }
        if(gameManager.GetLanguage() == Language.English)
        {
            nameLoot = nameLootEnglish;
        }
        else if(gameManager.GetLanguage() == Language.Italian)
        {
            nameLoot = nameLootItalian;
        }

        if (tipoLoot == TipoLoot.Tool && lootSpawnPoint != null)
        { 
            foreach (var lootPrefab in lootPrefabs)
            {
                GameObject loot = Instantiate(lootPrefab, lootSpawnPoint.position, lootSpawnPoint.rotation);
                loot.SetActive(true);
            }
            if(main) GameEvents.Instance.onNewLootAcquired.Invoke(nameLoot, tipoLoot);
        }
        else if(tipoLoot == TipoLoot.Card && lootSpawnPoint != null)
        {
            foreach (var lootPrefab in lootPrefabs)
            {
                SpawnCard card = lootPrefab.GetComponent<SpawnCard>();
                if (card != null)
                {
                    if(card.tipoCard == TipoCard.FormulaCard)
                    {
                        card.SpawnFormulaCard();
                    }
                    else if(card.tipoCard == TipoCard.DataCard)
                    {
                        card.SpawnDataCard();
                    }
                }
            }
            if(main) GameEvents.Instance.onNewLootAcquired.Invoke(nameLoot, tipoLoot);
            
        }
         else if(tipoLoot == TipoLoot.Button)
        {
            foreach (var lootPrefab in lootPrefabs)
            {
                InventoryButton button = lootPrefab.GetComponent<InventoryButton>();
                if (button != null)
                {
                    button.SetActive(true);
                }
            }
            if(main) GameEvents.Instance.onNewLootAcquired.Invoke(nameLoot, tipoLoot);
            
         }

    }
}
