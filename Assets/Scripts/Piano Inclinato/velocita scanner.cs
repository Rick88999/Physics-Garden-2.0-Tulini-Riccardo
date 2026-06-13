using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class velocitascanner : MonoBehaviour
{
    [SerializeField] public float minimumvelocity = 0.05f;
    [SerializeField] public float detectionValue = 0;
    [SerializeField] public TextMeshProUGUI velocitaText;
    [SerializeField] private GameObject DataCardSpawner;
    [SerializeField] private GameObject manopolaAngolo;
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private bool wasObjectInSocket;
    [SerializeField] private bool savedStateSocket;
    private string ultimoOggetto = "Sconosciuto";

    public void SaveSateSocket() => savedStateSocket = wasObjectInSocket;

    private void Start()
    {
        if (socket != null)
        {
            socket.selectEntered.AddListener(OnObjectInSocket);
            socket.selectExited.AddListener(OnObjectRemovedFromSocket);
        }
    }

    private void OnObjectInSocket(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.GetComponent<SphereCollider>() != null)
        {
            wasObjectInSocket = true;
            ultimoOggetto = "Sfera";
        }
        else if (args.interactableObject.transform.GetComponent<CapsuleCollider>() != null)
        {
            wasObjectInSocket = true;
            ultimoOggetto = "Cilindro";
        }
        else
        {
            wasObjectInSocket = false;
            ultimoOggetto = "Sconosciuto";
        }

        Debug.Log("Oggetto inserito nella socket");
    }

    private void OnObjectRemovedFromSocket(SelectExitEventArgs args)
    {
        wasObjectInSocket = false;
        Debug.Log("Oggetto rimosso dalla socket");
    }


    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        float velocitaFinale;
        float accelerazioneGravitazionale = 9.81f;
        float lunghezzaPianoInclinato = 1.5f;
        float costanteInerzialeSfera = 0.4f;
        float costanteInerzialeCilindro = 0.5f;

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
            
            if(tipoOggetto == "Sfera" && ultimoOggetto == "Sfera" && savedStateSocket)
            {
                velocitaFinale = Mathf.Sqrt((2 * accelerazioneGravitazionale * lunghezzaPianoInclinato * Mathf.Sin(angoloPiano * Mathf.Deg2Rad))/(1+costanteInerzialeSfera));
                velocitaText.text = $"{velocitaFinale:F1} m/s";
                DataCardSpawner.GetComponent<SpawnCard>().ValueReceiver(Mathf.Round(velocitaFinale * 100f) / 100f);
            }
            else if (tipoOggetto == "Cilindro" && ultimoOggetto == "Cilindro" && savedStateSocket)
            {
                velocitaFinale = Mathf.Sqrt((2 * accelerazioneGravitazionale * lunghezzaPianoInclinato * Mathf.Sin(angoloPiano * Mathf.Deg2Rad)) / (1 + costanteInerzialeCilindro));
                velocitaText.text = $"{velocitaFinale:F2} m/s";
                DataCardSpawner.GetComponent<SpawnCard>().ValueReceiver(Mathf.Round(velocitaFinale * 100f) / 100f);
            }
            else
            {
                velocitaText.text = "Error: Retry";
                DataCardSpawner.GetComponent<SpawnCard>().ValueReceiver(0);
            }
            savedStateSocket = false;
            wasObjectInSocket = false;



            DataCardSpawner.GetComponent<SpawnCard>().DetailsReceiver($"{tipoOggetto:F1}");
            DataCardSpawner.GetComponent<SpawnCard>().SpawnDataCard();

        }

    }
}
