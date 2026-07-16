using System.Collections;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
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

    [Header("Parametri Fisici")]
    [SerializeField] private float velocitaFinale;
    [SerializeField] private float accelerazioneGravitazionale = 9.81f;
    [SerializeField] private float lunghezzaPianoInclinato = 1.5f;
    [SerializeField] private float costanteInerzialeSfera = 0.4f;
    [SerializeField] private float costanteInerzialeCilindro = 0.5f;
    private string ultimoOggetto = "Sconosciuto";
    private GameObject lastObjectInSocket;
    private int lastObjectId;
    private bool abortTriggerExecution;

    public void SaveStateSocket()
    {
        savedStateSocket = wasObjectInSocket;
        lastObjectId = lastObjectInSocket != null ? lastObjectInSocket.GetInstanceID() : -1;
    }

    private void Start()
    {
        if (socket != null)
        {
            socket.selectEntered.AddListener(OnObjectInSocket);
            socket.selectExited.AddListener(OnObjectRemovedFromSocket);
            abortTriggerExecution = false;
            GameEvents.Instance.onInclinedPlaneObjectDestroyed.AddListener(OnInclinedPlaneObjectDestroyed);
        }
    }

    private void OnObjectInSocket(SelectEnterEventArgs args)
    {
        abortTriggerExecution = false;

        if (args.interactableObject.transform.GetComponent<SphereCollider>() != null)
        {
            wasObjectInSocket = true;
            lastObjectInSocket = args.interactableObject.transform.gameObject;
            ultimoOggetto = "Sfera";
        }
        else if (args.interactableObject.transform.GetComponent<CapsuleCollider>() != null)
        {
            wasObjectInSocket = true;
            lastObjectInSocket = args.interactableObject.transform.gameObject;
            ultimoOggetto = "Cilindro";
        }
        else
        {
            wasObjectInSocket = false;
            lastObjectInSocket = null;
            ultimoOggetto = "Sconosciuto";
        }

        Debug.Log("Oggetto inserito nella socket");
    }

    private void OnObjectRemovedFromSocket(SelectExitEventArgs args)
    {
        abortTriggerExecution = false;
        wasObjectInSocket = false;
        lastObjectInSocket = null;
        Debug.Log("Oggetto rimosso dalla socket");
    }

    private void OnInclinedPlaneObjectDestroyed(int objectId)
    {
        if (lastObjectInSocket != null && lastObjectInSocket.GetInstanceID() == objectId)
        {
            wasObjectInSocket = false;
            lastObjectInSocket = null;
            abortTriggerExecution = true;
            Debug.Log("Oggetto distrutto e rimosso dalla socket");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if(lastObjectId == -1)
        {
            return;
        }

        if(other.transform.gameObject.GetInstanceID() != lastObjectId)
        {
            return;
        }
        

        if (abortTriggerExecution)
        {
            abortTriggerExecution = false;
            return;
        }

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

            if (abortTriggerExecution)
            {
                abortTriggerExecution = false;
                return;
            }

            float angolo =  manopolaAngolo.GetComponent<RuotaPiano>().GetCurrentPianoAngle();
            float angoloPiano = Mathf.RoundToInt(angolo);

            if (abortTriggerExecution)
            {
                abortTriggerExecution = false;
                return;
            }
            
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

            if (abortTriggerExecution)
            {
                abortTriggerExecution = false;
                return;
            }

            DataCardSpawner.GetComponent<SpawnCard>().DetailsReceiver($"{tipoOggetto:F1}");
            DataCardSpawner.GetComponent<SpawnCard>().SpawnDataCard();

            StartCoroutine(ResetGrabInteractable(other));

            

        }
    }

    private IEnumerator ResetGrabInteractable(Collider other)
    {
        yield return new WaitForSeconds(0.3f);
        other.transform.gameObject.layer = LayerMask.NameToLayer("OggettiPianoInclinato");

        if (other.transform.gameObject.GetComponent<XRGrabInteractable>() != null)
        {
            if (!other.transform.gameObject.GetComponent<XRGrabInteractable>().enabled)
            {
                other.transform.gameObject.GetComponent<XRGrabInteractable>().enabled = true;
            }
        }
    }
}
