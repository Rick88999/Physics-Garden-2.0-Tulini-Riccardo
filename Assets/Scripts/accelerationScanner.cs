using UnityEngine;
using TMPro;

public class accelerationScanner : MonoBehaviour
{
    [SerializeField] public TimeAnsVelocityScanner tracker;
    [SerializeField] public float minimumAcceleration = 0.1f;
    [SerializeField] public float detectionValue = 0;
    [SerializeField] public TextMeshProUGUI accelerationText;

    public float acceleration { get; private set; }



    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null && tracker != null)
        {
            float velocitaFinale = rb.linearVelocity.magnitude;

            float tempoIniziale = tracker.tempoIniziale;
            float velocitaIniziale = tracker.velocitaIniziale.magnitude;

            acceleration = (velocitaFinale - velocitaIniziale) / (Time.time - tempoIniziale);
            accelerationText.text = "Acceleration: " + acceleration.ToString("F2") + " m/s²";
        }
        
    }
}
