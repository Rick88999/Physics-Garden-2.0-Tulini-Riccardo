using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    [SerializeField] public GameObject terminale;

    public void DeactiveTerminal()
    {
        terminale.SetActive(false);
    }

    public void SpawnTerminal(Vector3 position, Quaternion rotation)
    {
        terminale.transform.position = position;
        terminale.transform.rotation = rotation;
    }

    public void ActiveTerminal() => terminale.SetActive(true);
    
}
