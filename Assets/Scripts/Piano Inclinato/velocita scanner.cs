using TMPro;
using UnityEngine;

public class velocitascanner : MonoBehaviour
{
    [SerializeField] public float minimumvelocity = 0.05f;
    [SerializeField] public float detectionValue = 0;
    [SerializeField] public TextMeshProUGUI velocitaText;
    [SerializeField] private GameObject DataCardSpawner;
    [SerializeField] private GameObject manopolaAngolo;
    private float velocitaVera;

 
    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        float velocitaFinale;
        float accelerazioneGravitazionale = 9.81f;
        float lunghezzaPianoInclinato = 1.5f;
        float costanteInerzialeSfera = 0.5f;
        float costanteInerzialeCilindro = 0.4f;

        if (rb != null)
        {
            string tipoOggetto = "Sconosciuto";

            if (other.GetComponent<SphereCollider>() != null)
            {
                tipoOggetto = "Sfera";
            }
            else if (other.GetComponent<CapsuleCollider>() != null)
            {
                tipoOggetto = "Cilindro";
                
            }
            float angolo =  manopolaAngolo.GetComponent<RuotaPiano>().GetCurrentPianoAngle();
            float angoloPiano = Mathf.RoundToInt(angolo);
            
            if(tipoOggetto == "Sfera")
            {
                velocitaFinale = Mathf.Sqrt((2 * accelerazioneGravitazionale * lunghezzaPianoInclinato * Mathf.Sin(angoloPiano * Mathf.Deg2Rad))/(1+costanteInerzialeSfera));
                velocitaText.text = $"Velocita finale: {velocitaFinale:F1} m/s";
                DataCardSpawner.GetComponent<SpawnCard>().ValueReceiver(Mathf.Round(velocitaFinale * 100f) / 100f);
            }
            else if (tipoOggetto == "Cilindro")
            {
                velocitaFinale = Mathf.Sqrt((2 * accelerazioneGravitazionale * lunghezzaPianoInclinato * Mathf.Sin(angoloPiano * Mathf.Deg2Rad)) / (1 + costanteInerzialeCilindro));
                velocitaText.text = $"Velocita finale: {velocitaFinale:F2} m/s";
                DataCardSpawner.GetComponent<SpawnCard>().ValueReceiver(Mathf.Round(velocitaFinale * 100f) / 100f);
            }

            DataCardSpawner.GetComponent<SpawnCard>().DetailsReceiver($"{tipoOggetto:F1}");
            DataCardSpawner.GetComponent<SpawnCard>().SpawnDataCard();

        }

    }
}
