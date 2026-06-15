using System.Collections;
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

    [SerializeField] private AudioSource dialogAudioSourceError;
    [SerializeField] private AudioClip[] sentenceClipsError;

    private Coroutine typingCoroutineError;
    private bool isTypingError = false;
    private Language actualLenguageError;

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
        Reset();
        canvasPointerError.SetActive(true);
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
    }

    private void Reset()
    {
        indexError = 0;
        alreadyStartedError = false;
        isTypingError = false;
        botTextError.text = "";
    }


}
