using System.Collections;
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
            StartCoroutine(EndStepDelay());
        }
    }

    private IEnumerator EndStepDelay()
    {
        yield return new WaitForSeconds(1.5f);
        step.endStep();
    }
}
