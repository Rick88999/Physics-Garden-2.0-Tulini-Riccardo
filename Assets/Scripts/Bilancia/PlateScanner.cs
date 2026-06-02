using UnityEngine;

public class PlateScanner : MonoBehaviour
{
    [SerializeField] private BilanciaDuePiatti bilancia;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tool") || other.CompareTag("PlayerHand")) return;
        bilancia.ObjectEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tool") || other.CompareTag("PlayerHand")) return;
        bilancia.ObjectExit(other.gameObject);
    }
}
