using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LootEvent : UnityEvent<string, TipoLoot> { }

[System.Serializable]
public class ToolEvent : UnityEvent<string> { }

[System.Serializable]
public class  ChangeHandEvent : UnityEvent<string> { }

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }
    public LootEvent onNewLootAcquired;
    public ToolEvent onToolSpawned;
    public ChangeHandEvent onChangeHand;
    public UnityEvent onDropTool;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        onNewLootAcquired ??= new LootEvent();
        onToolSpawned ??= new ToolEvent();
        onChangeHand ??= new ChangeHandEvent();
        onDropTool ??= new UnityEvent();
    }
}
