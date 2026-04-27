using UnityEngine;

public class TimeAnsVelocityScanner : MonoBehaviour
{
    [SerializeField] private float minVelocity =0.1f;
    public float tempoIniziale { get; private set; }
    private Rigidbody rb;
    public Vector3 velocitaIniziale { get; private set; }

    public void OnTriggerEnter(Collider other)
    {
        
        rb = other.GetComponent<Rigidbody>();
        if (rb != null && rb.linearVelocity.magnitude > minVelocity)
        {
            tempoIniziale = Time.time;
            velocitaIniziale = rb.linearVelocity;
        }
        
    }
}

