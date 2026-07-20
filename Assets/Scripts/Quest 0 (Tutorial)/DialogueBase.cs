using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class DialogueBase : MonoBehaviour
{
    [FormerlySerializedAs("gameManager")][FormerlySerializedAs("gameManagerError")]
    public GameManager gameManager;

    [FormerlySerializedAs("botText")][FormerlySerializedAs("botTextError")]
    public TextMeshProUGUI botText;

    [FormerlySerializedAs("canvasPointer")][FormerlySerializedAs("canvasPointerError")]
    public GameObject canvasPointer;

    [FormerlySerializedAs("bot")][FormerlySerializedAs("botError")]
    public GameObject bot;

    [FormerlySerializedAs("typingSpeed")][FormerlySerializedAs("typingSpeedError")]
    [SerializeField] protected float typingSpeed = 0.05f;

    [FormerlySerializedAs("ItalianSentences")][FormerlySerializedAs("ItalianSentencesError")]
    [SerializeField] private string[] ItalianSentences;

    [FormerlySerializedAs("EnglishSentences")][FormerlySerializedAs("EnglishSentencesError")]
    [SerializeField] private string[] EnglishSentences;
    
    [FormerlySerializedAs("dialogAudioSource")][FormerlySerializedAs("dialogAudioSourceError")]
    [SerializeField] private AudioSource dialogAudioSource;

    [FormerlySerializedAs("sentenceClips")][FormerlySerializedAs("sentenceClipsError")]
    [SerializeField] private AudioClip[] sentenceClips;

    protected string[] sentences;
    protected int index = 0;
    protected bool alreadyStarted = false;
    protected bool isTyping = false;
    
    private Coroutine typingCoroutine;
    protected Language actualLanguage;

    public virtual void Awake()
    {
        actualLanguage = gameManager.GetLanguage();
        if (actualLanguage == Language.Italian)
        {
            sentences = ItalianSentences;
        }
        else if (actualLanguage == Language.English)
        {
            sentences = EnglishSentences;
        }
    }

    public virtual void nextSentence()
    {
        if (isTyping)
        {
            FinishCurrentSentenceInstant();
            return;
        }

        if (index == 0 && !alreadyStarted)
        {
            alreadyStarted = true;
            StartTypingCurrent();
        }
        else if (alreadyStarted && index < sentences.Length - 1)
        {
            index++;
            StartTypingCurrent();
        }
        else
        {
            EndDialogue();
        }
    }

    protected virtual void StartTypingCurrent()
    {
        botText.text = "";

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        PlayClipForCurrent();
        typingCoroutine = StartCoroutine(TypeRoute());
    }

    private void FinishCurrentSentenceInstant()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        botText.text = sentences[index];
        isTyping = false;

        if (dialogAudioSource != null && dialogAudioSource.isPlaying)
            dialogAudioSource.Stop();
    }

    private void PlayClipForCurrent()
    {
        if (dialogAudioSource == null) return;
        if (sentenceClips == null || index >= sentenceClips.Length) return;
        AudioClip clip = sentenceClips[index];
        if (clip == null) return;

        dialogAudioSource.Stop();
        dialogAudioSource.clip = clip;
        dialogAudioSource.Play();
    }

    private IEnumerator TypeRoute()
    {
        isTyping = true;

        foreach (char letter in sentences[index])
        {
            botText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;

        if (dialogAudioSource != null && dialogAudioSource.isPlaying)
            dialogAudioSource.Stop();

        OnSentenceFinished();
    }

    protected virtual void OnSentenceFinished() { }

    protected abstract void EndDialogue();

    protected virtual void ResetDialogue()
    {
        index = 0;
        alreadyStarted = false;
        isTyping = false;
        botText.text = "";
    }
}
