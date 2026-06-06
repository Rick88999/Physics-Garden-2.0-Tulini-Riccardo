using UnityEngine;

public class MassaCounterBox : MonoBehaviour
{
    private BoxCollider boxCollider;
    private float massaTotale = 0f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("MassaCounterBox: Non è stato trovato un Collider sul GameObject.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            massaTotale += rb.mass;
            Debug.Log("Massa totale nella box: " + massaTotale);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            massaTotale -= rb.mass;
            Debug.Log("Massa totale nella box: " + massaTotale);
        }
    }

    public float GetMassaTotale()
    {
        if (massaTotale == 0) return 1;
        else return massaTotale;
    }
}

