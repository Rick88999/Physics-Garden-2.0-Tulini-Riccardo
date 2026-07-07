using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject Portello;
    [SerializeField] private Vector3 targetPostAnimation;
    [SerializeField] private Vector3 targetPostAnimationRotation;
    private bool noPerformAnimation = false;
    
    private void Awake() => StartFlying();
    private void StartFlying()
    {
        if (noPerformAnimation) return;
        audioSource.PlayOneShot(audioClip);
        gameObject.GetComponent<Animator>().enabled = true;
    }
    public void SetPostAnimationPosition()
    {
        transform.position = targetPostAnimation;
        Portello.transform.localRotation = Quaternion.Euler(targetPostAnimationRotation);  
    }
    public void SetNoPerformAnimation() => noPerformAnimation = true;  
}
