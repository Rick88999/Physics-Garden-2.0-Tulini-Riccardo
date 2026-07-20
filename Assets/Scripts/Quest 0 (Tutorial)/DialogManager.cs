using UnityEngine;

public class DialogManager : DialogueBase
{
    public Step fatherStep;
    public bool afterEndStep;
    public bool canvasEndToggle = true;
    [SerializeField] public GameObject[] toActivateAtEnd;

    public override void Awake()
    {
        base.Awake();
        canvasPointer.SetActive(true);
        nextSentence();
    }

    protected override void EndDialogue()
    {
        if (canvasEndToggle)
        {
            botText.text = "";
            if (toActivateAtEnd != null)
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
            var controller = bot.GetComponent<BotController>();
            if (controller != null && controller.getMode() == "GoToPoint") 
                controller.StartFollow();
        }

        if (afterEndStep && fatherStep != null) 
            fatherStep.endStep();
    }
}