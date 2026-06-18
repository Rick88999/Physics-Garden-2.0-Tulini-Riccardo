using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LookTarget : MonoBehaviour
{
    public UnityEvent onActivated;
    [SerializeField] private float resetDelay = 2f;
    private bool triggered = false;

    public void Activate()
    {
        if (triggered) return;

        triggered = true;
        onActivated.Invoke();
        StartCoroutine(ResetTriggeredAfterDelay(resetDelay));
    }

    private IEnumerator ResetTriggeredAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        triggered = false;
    }
}
