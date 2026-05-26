using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private GameObject FatherPanel;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private bool state = false;

    public void SetActive(bool oldState)
    {
        state = oldState;
        inventoryButton.SetActive(state);
    }

    public void AwakeBotton() => inventoryButton.SetActive(state);

    public bool GetState() => state;
}