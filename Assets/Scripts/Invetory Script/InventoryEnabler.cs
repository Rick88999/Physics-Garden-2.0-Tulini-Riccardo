using UnityEngine;

public class InventoryEnabler : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    public void Awake()
    {
        if (inventoryManager != null)
        {
            inventoryManager.EnableInventory();
        }
    }
}
