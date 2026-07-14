using UnityEngine;

public class PlateScanner : MonoBehaviour
{
    [SerializeField] private BilanciaDuePiatti bilancia;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tool") || other.CompareTag("PlayerHand")) return;
        other.tag = "No Destroy";
        bilancia.ObjectEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tool") || other.CompareTag("PlayerHand")) return;
        other.tag = "Spawned Object";
        bilancia.ObjectExit(other.gameObject);
    }
}
