using UnityEngine;

public class TriggerAvvicinamento : MonoBehaviour
{
    [SerializeField] public GameObject bot;
    [SerializeField] public Transform punto;
    public Step step;

    private void OnTriggerEnter(Collider other)
    {
        bot.GetComponent<BotController>().GoToFixedPoint(punto);
        step.endStep();

        
    }
}
