using UnityEngine;

public class TriggerAvvicinamento : MonoBehaviour
{
   [SerializeField] public GameObject bot;
    [SerializeField] public Transform punto;
    public Step step;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        bot.GetComponent<BotController>().GoToFixedPoint(punto);
        step.endStep();

        
    }
}
