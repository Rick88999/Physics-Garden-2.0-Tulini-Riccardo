using UnityEngine;

public class TriggerAvvicinamento : MonoBehaviour
{
    [SerializeField] public GameObject bot;
    [SerializeField] public Transform punto;

    private void OnTriggerEnter(Collider other) =>bot.GetComponent<BotController>().GoToFixedPoint(punto);
    
}
