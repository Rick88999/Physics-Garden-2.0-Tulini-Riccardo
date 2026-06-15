using UnityEngine;

public class StariDialogue : MonoBehaviour
{
    public Step nextStep;
    public GameObject collider;
    
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nextStep.startStep();
            collider.SetActive(false);
        }
    }
}
