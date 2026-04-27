using NUnit.Framework.Constraints;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] Quest[] quests;
    private int currentQuest = 0;

    public void startQuest(int number)
    {
        currentQuest = number;
        quests[number].startQuest();
    }

    public void settingsSaves(string[] questData, string[] stepData)
    {
        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].setQuestState(questData[i]);
            for (int j = 0; j < quests[i].steps.Length; j++)
            {
                quests[i].steps[j].setState(stepData[j]);
            }
        }
    }
}
