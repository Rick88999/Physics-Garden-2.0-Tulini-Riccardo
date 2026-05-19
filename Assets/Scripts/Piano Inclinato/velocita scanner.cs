using TMPro;
using UnityEngine;

public class velocitascanner : MonoBehaviour
{
    [SerializeField] public float minimumvelocity = 0.05f;
    [SerializeField] public float detectionValue = 0;
    [SerializeField] public TextMeshProUGUI velocitaText;
    [SerializeField] private GameObject DataCardSpawner;
    private float velocitaVera;

 
    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            float velocitaFinale = rb.linearVelocity.magnitude;
            
            velocitaText.text = $"Velocita finale: {velocitaFinale:F3} m/s";

            DataCardSpawner.GetComponent<SpawnCard>().ValueReceiver(Mathf.Round(velocitaFinale * 10f) / 10f);
            DataCardSpawner.GetComponent<SpawnCard>().SpawnDataCard();

        }

    }
}
