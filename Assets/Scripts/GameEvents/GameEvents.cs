using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LootEvent : UnityEvent<string, TipoLoot> { }

[System.Serializable]
public class ToolEvent : UnityEvent<string> { }

[System.Serializable]
public class  ChangeHandEvent : UnityEvent<string, int> { }

[System.Serializable]

public class DropToolEvent : UnityEvent<int> { }

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }
    public LootEvent onNewLootAcquired;
    public ToolEvent onToolSpawned;
    public ChangeHandEvent onChangeHandMassTool;
    public ChangeHandEvent onChangeHandSpawnerTool;
    public DropToolEvent onDropMassTool;
    public DropToolEvent onDropSpawnerTool;
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
        onChangeHandMassTool ??= new ChangeHandEvent();
        onChangeHandSpawnerTool ??= new ChangeHandEvent();
        onDropMassTool ??= new DropToolEvent();
        onDropSpawnerTool ??= new DropToolEvent();
    }
}
