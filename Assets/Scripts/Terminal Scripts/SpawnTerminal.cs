using UnityEngine;

public class SpawnTerminal : MonoBehaviour
{
    [SerializeField] private TerminalManager terminalManager;
    [SerializeField] private Transform spawnPosition;

    public void Spawn()
    {
        if(terminalManager.terminale.transform.position  != spawnPosition.position)
            terminalManager.SpawnTerminal(spawnPosition.position, spawnPosition.rotation);
        terminalManager.ActiveTerminal();
    }
    public void Deactive() => terminalManager.DeactiveTerminal();
    
}
