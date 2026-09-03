using UnityEngine;
using UnityEngine.Serialization;

public class SpawnTerminal : MonoBehaviour
{
    [FormerlySerializedAs("terminalManager")]
    [SerializeField] private TerminalCallerManager terminalCallerManager;
    [SerializeField] private TerminalController terminalController;
    [SerializeField] private Transform spawnPosition;
    private VRInterruptor vrInterruptor;
    public void Spawn()
    {
        if(terminalController.terminale.transform.position  != spawnPosition.position)
            terminalController.SpawnTerminal(spawnPosition.position, spawnPosition.rotation);
        terminalController.ActiveTerminal();
    }

    public void SetLastTerminalCaller(GameObject terminalCaller)
    {
        terminalCallerManager.SetLastTerminalCaller(terminalCaller);
    }
    public void Deactive() => terminalController.DeactiveTerminal();

    public bool CardAlreadyInSocket() => terminalController.IsCardInSocket();

    public void PlayAudioClip() => terminalController.PlayAudioClip();
    
}
