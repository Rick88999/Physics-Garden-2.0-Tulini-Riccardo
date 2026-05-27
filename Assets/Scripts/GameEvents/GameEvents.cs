using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LootEvent : UnityEvent<string, TipoLoot> { }

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }
    public LootEvent onNewLootAcquired;

    void Awake() => Instance = this;
}
