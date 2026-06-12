using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DialogManager : MonoBehaviour
{
    public GameManager gameManager;
    public Step fatherStep;
    public TextMeshProUGUI botText;
    public GameObject panel;
    public GameObject canvasPointer;
    public GameObject bot;
    public bool afterEndStep;
    public bool canvasEndToggle = true;


    private int index = 0;
    private bool alreadyStarted = false;

    [SerializeField] public float typingSpeed;
    private string[] sentences;
    [SerializeField] private string[] ItalianSentences;
    [SerializeField] private string[] EnglishSentences;
    [SerializeField] public GameObject[] toActivateAtEnd;

    [SerializeField] private AudioSource dialogAudioSource;
    [SerializeField] private AudioClip[] sentenceClips;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private Lenguage actualLenguage;

    public void Awake()
    {
        actualLenguage = gameManager.GetLenguage();
        if(actualLenguage == Lenguage.Italian)
        {
            sentences = ItalianSentences;
        }
        else if(actualLenguage == Lenguage.English)
        {
            sentences = EnglishSentences;
        }

        canvasPointer.SetActive(true);
        nextSentence();
    }

    public void nextSentence()
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
            
            if (canvasEndToggle)
            {
                botText.text = "";
                if(toActivateAtEnd != null)
                {
                    foreach (GameObject obj in toActivateAtEnd)
                    {
                        obj.SetActive(true);
                    }
                }
                canvasPointer.SetActive(false);
            }

            if (bot.name == "Bot")
            {
                if(bot.GetComponent<BotController>().getMode() == "GoToPoint") bot.GetComponent<BotController>().StartFollow();
            }

            if(afterEndStep) fatherStep.endStep();
        }
    }

    private void StartTypingCurrent()
    {
        botText.text = "";

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        PlayClipForCurrent();

        typingCoroutine = StartCoroutine(Type());
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

    IEnumerator Type()
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
    }

    
}