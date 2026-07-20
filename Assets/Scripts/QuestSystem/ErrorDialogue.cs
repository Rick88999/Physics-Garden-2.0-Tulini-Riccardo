using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorDialogue : DialogueBase
{
    [SerializeField] private GameObject point;
    [SerializeField] private bool oneWayAddOn;
    [SerializeField] private List<GameObject> ways;
    [SerializeField] private float autoNextDelay = 10f;

    private Coroutine autoNextCoroutineError;
    private bool initialPointState;

    public void StarterDialogueError()
    {
        if (oneWayAddOn && ways != null && ways.Count > 0) 
        {
            foreach (GameObject way in ways) way.SetActive(false);
        }

        ResetDialogue();
        canvasPointer.SetActive(true);
        initialPointState = point.activeSelf;
        point.SetActive(false);
        nextSentence();
    }

    protected override void OnSentenceFinished()
    {
        if (index == sentences.Length - 1)
        {
            if (autoNextCoroutineError != null) StopCoroutine(autoNextCoroutineError);
            autoNextCoroutineError = StartCoroutine(AutoNextError());
        }
    }

    protected override void EndDialogue()
    {
        botText.text = "";
        canvasPointer.SetActive(false);
        ResetDialogue();
        gameObject.SetActive(false);
        point.SetActive(initialPointState);
    }

    private IEnumerator AutoNextError()
    {
        yield return new WaitForSeconds(autoNextDelay);
        autoNextCoroutineError = null;
        nextSentence();
    }

    protected override void ResetDialogue()
    {
        base.ResetDialogue();
        if (autoNextCoroutineError != null)
        {
            StopCoroutine(autoNextCoroutineError);
            autoNextCoroutineError = null;
        }
    }
}