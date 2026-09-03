using UnityEngine;

public class TerminalController : MonoBehaviour
{
    [SerializeField] public GameObject terminale;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private bool isCardInSocket = false;

    public void DeactiveTerminal() => terminale.SetActive(false);

    public bool IsCardInSocket()
    {
        ScanCardInSocket();
        return isCardInSocket;
    }

    public void PlayAudioClip()
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void ScanCardInSocket()
    {
        CanvasLoader canvasLoader = terminale.GetComponentInChildren<CanvasLoader>();

        if(canvasLoader != null)
        {
            isCardInSocket = canvasLoader.IsCardInSocket();
        }

    }
    
    public void SpawnTerminal(Vector3 position, Quaternion rotation)
    {
        terminale.transform.position = position;
        terminale.transform.rotation = rotation;
    }

    public void ActiveTerminal() => terminale.SetActive(true);
    
}
