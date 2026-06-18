using UnityEngine;

public class BasketController : MonoBehaviour
{
    [SerializeField] private Step step;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip catchSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            audioSource.PlayOneShot(catchSound);
            step.endStep();
        }
    }
}
