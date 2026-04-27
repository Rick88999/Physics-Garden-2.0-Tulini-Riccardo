using UnityEngine;

public class startCollider : MonoBehaviour
{
   public QuestManager questManager;
    [SerializeField] private int number;

    public void OnTriggerEnter(Collider other)
    {
        
            questManager.startQuest(number);
            Destroy(gameObject);
        
    }
}
