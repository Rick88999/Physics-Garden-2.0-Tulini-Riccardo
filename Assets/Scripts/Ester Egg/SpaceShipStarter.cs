using UnityEngine;

public class SpaceShipStarter : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    
    private void Awake() => StartFlying();
    private void StartFlying()
    {
        audioSource.PlayOneShot(audioClip);
        gameObject.GetComponent<Animator>().enabled = true;
    }
}
