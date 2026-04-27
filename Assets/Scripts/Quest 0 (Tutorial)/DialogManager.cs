using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DialogManager : MonoBehaviour
{
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
    [SerializeField] private string[] sentences;
    [SerializeField] public GameObject[] toActivateAtEnd;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    public void Awake()
    {
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
    }

    
}