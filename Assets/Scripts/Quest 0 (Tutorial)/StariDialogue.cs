using UnityEngine;

public class StariDialogue : MonoBehaviour
{
    public Step actualStep;
    public GameObject collider;
    
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actualStep.endStep();
            collider.SetActive(false);
        }
    }
}
