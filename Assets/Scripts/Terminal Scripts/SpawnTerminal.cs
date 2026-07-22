using UnityEngine;
using UnityEngine.Serialization;

public class SpawnTerminal : MonoBehaviour
{
    [FormerlySerializedAs("terminalManager")]
    [SerializeField] private TerminalController terminalController;
    [SerializeField] private Transform spawnPosition;

    public void Spawn()
    {
        if(terminalController.terminale.transform.position  != spawnPosition.position)
            terminalController.SpawnTerminal(spawnPosition.position, spawnPosition.rotation);
        terminalController.ActiveTerminal();
    }

    public void Deactive() => terminalController.DeactiveTerminal();
    
}
