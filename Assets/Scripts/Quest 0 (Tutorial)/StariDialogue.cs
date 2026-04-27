using UnityEngine;

public class StariDialogue : MonoBehaviour
{
    public GameObject manager;
    public GameObject collider;
    
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.SetActive(true);
            manager.GetComponent<DialogManager>().nextSentence();
            collider.SetActive(false);
        }
    }
}
