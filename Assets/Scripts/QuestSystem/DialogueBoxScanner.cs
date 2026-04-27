using UnityEngine;

public class DialogueBoxScanner : MonoBehaviour
{
    public void scanForDialogueBox()
    {
        DialogManager dialogueBox = Object.FindFirstObjectByType<DialogManager>();
        if (dialogueBox != null)
        {
            dialogueBox.nextSentence();
        }
    }
        
}
