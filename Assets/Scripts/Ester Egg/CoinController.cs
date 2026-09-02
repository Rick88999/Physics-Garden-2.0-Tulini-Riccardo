using System.Collections;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private float delayBeforeActivation = 0.2f;
    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        StartCoroutine(ActivateCoin());
    }

    public  IEnumerator ActivateCoin()
    {
        yield return new WaitForSeconds(delayBeforeActivation);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        yield return null;
    }
}
