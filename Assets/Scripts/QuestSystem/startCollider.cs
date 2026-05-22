using UnityEngine;

public class StartCollider : MonoBehaviour
{
    public QuestManager questManager;
    [SerializeField] private int number;
    [SerializeField] private GameObject[] members;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        questManager.startQuest(number);
        DestroyTriggers();
    }

    public void DestroyTriggers()
    {
        if (members == null || members.Length == 0) return;
        foreach (GameObject member in members) Destroy(member);
    }
}
