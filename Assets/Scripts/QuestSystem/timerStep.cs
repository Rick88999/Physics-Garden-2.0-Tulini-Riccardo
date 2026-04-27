using System.Collections;
using UnityEngine;

public class timerStep : MonoBehaviour
{
    public Step step;
    public void timerNextStep(float time)
    {
        StartCoroutine(timer(time));
    }

    private IEnumerator timer(float time)
    {
        yield return new WaitForSeconds(time);
        step.endStep();
    }
}