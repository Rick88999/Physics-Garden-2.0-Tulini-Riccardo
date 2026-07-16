using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ErrorDialogue : MonoBehaviour
{
    public GameManager gameManagerError;

    public TextMeshProUGUI botTextError;
    public GameObject panelError;
    public GameObject canvasPointerError;
    public GameObject botError;


    private int indexError = 0;
    private bool alreadyStartedError = false;

    [SerializeField] public float typingSpeedError;
    private string[] sentencesError;
    [SerializeField] private string[] ItalianSentencesError;
    [SerializeField] private string[] EnglishSentencesError;

    [SerializeField] private GameObject point;

    [SerializeField] private AudioSource dialogAudioSourceError;
    [SerializeField] private AudioClip[] sentenceClipsError;
    
    [SerializeField]private bool oneWayAddOn;
    [SerializeField] private List<GameObject> ways;
    [SerializeField] private float autoNextDelay = 10f;
    private Coroutine typingCoroutineError;
    private Coroutine autoNextCoroutineError;
    private bool isTypingError = false;
    private Language actualLenguageError;

    private bool initialPointState;
   

    public void Awake()
    {
        actualLenguageError = gameManagerError.GetLanguage();
        if (actualLenguageError == Language.Italian)
        {
            sentencesError = ItalianSentencesError;
        }
        else if (actualLenguageError == Language.English)
        {
            sentencesError = EnglishSentencesError;
        }
    }

    public void StarterDialogueError()
    {
        if(oneWayAddOn) 
            if(ways!=null || ways.Count>0) 
                foreach(GameObject way in ways) way.SetActive(false);

        Reset();
        canvasPointerError.SetActive(true);
        initialPointState = point.activeSelf;
        point.SetActive(false);
        nextSentenceError();
    }

    public void nextSentenceError()
    {

        if (isTypingError)
        {
            FinishCurrentSentenceInstantError();
            return;
        }

        if (indexError == 0 && !alreadyStartedError)
        {
            alreadyStartedError = true;
            StartTypingCurrentError();
        }
        else if (alreadyStartedError && indexError < sentencesError.Length - 1)
        {
            indexError++;
            StartTypingCurrentError();
        }
        else
        {

                botTextError.text = "";
                canvasPointerError.SetActive(false);
                Reset();
                gameObject.SetActive(false);
                point.SetActive(initialPointState);
        }
    }

    private void StartTypingCurrentError()
    {
        botTextError.text = "";

        if (typingCoroutineError != null)
            StopCoroutine(typingCoroutineError);

        PlayClipForCurrentError();

        typingCoroutineError = StartCoroutine(TypeError());
    }

    private void FinishCurrentSentenceInstantError()
    {
        if (typingCoroutineError != null)
        {
            StopCoroutine(typingCoroutineError);
            typingCoroutineError = null;
        }

        botTextError.text = sentencesError[indexError];
        isTypingError = false;

        if (dialogAudioSourceError != null && dialogAudioSourceError.isPlaying)
            dialogAudioSourceError.Stop();
    }

    private void PlayClipForCurrentError()
    {
        if (dialogAudioSourceError == null) return;
        if (sentenceClipsError == null || indexError >= sentenceClipsError.Length) return;
        AudioClip clip = sentenceClipsError[indexError];
        if (clip == null) return;

        dialogAudioSourceError.Stop();
        dialogAudioSourceError.clip = clip;
        dialogAudioSourceError.Play();
    }

    IEnumerator TypeError()
    {
        isTypingError = true;

        foreach (char letter in sentencesError[indexError])
        {
            botTextError.text += letter;
            yield return new WaitForSeconds(typingSpeedError);
        }

        isTypingError = false;
        typingCoroutineError = null;
        if (dialogAudioSourceError != null && dialogAudioSourceError.isPlaying)
            dialogAudioSourceError.Stop();

        if (indexError == sentencesError.Length - 1)
        {
            if (autoNextCoroutineError != null) StopCoroutine(autoNextCoroutineError);
            autoNextCoroutineError = StartCoroutine(AutoNextError());
        }
    }

    private IEnumerator AutoNextError()
    {
        yield return new WaitForSeconds(autoNextDelay);
        autoNextCoroutineError = null;
        nextSentenceError();
    }

    private void Reset()
    {
        indexError = 0;
        alreadyStartedError = false;
        isTypingError = false;
        botTextError.text = "";
        if (autoNextCoroutineError != null)
        {
            StopCoroutine(autoNextCoroutineError);
            autoNextCoroutineError = null;
        }
    }


}
