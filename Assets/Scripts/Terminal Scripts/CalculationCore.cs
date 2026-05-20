using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CalculationCore : MonoBehaviour
{
    private Dictionary<UnitaMisura, string> simboliUnita = new Dictionary<UnitaMisura, string>()
    {
        { UnitaMisura.Metri,       "m"  },
        { UnitaMisura.Centimetri,  "cm" },
        { UnitaMisura.Millimetri,  "mm" },
        { UnitaMisura.Velocita,    "m/s" },
        { UnitaMisura.Angolo,      "°"  },
        { UnitaMisura.Kilogrammi, "kg" },
        { UnitaMisura.Accelerazione, "m/s<sup>2</sup>" },
        { UnitaMisura.None, "" }
    };

    [Header("Sockets per i valori")]
    [SerializeField]XRSocketInteractor[] sockets;

    [Header("Prefab della card risultato (DataCard)")]
    [SerializeField] GameObject resultCardPrefab;

    [Header("Puntatori per la spawn della card risultato")]
    [SerializeField] XRSocketInteractor spawnSocket;
    [SerializeField] Transform spawnResultCard;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSoundError;
    [SerializeField] private AudioClip clickSoundSuccess;

    [Header("Script per il cambio pagina")]
    [SerializeField] changePage changePageScript;

    private Dictionary<string, float> value;
    private float result;
    
    private string unitaMisura;
    private float velocitaReale;


    private void eliminaOggettiInSockets()
    {
        foreach (XRSocketInteractor socket in sockets)
        {
            if (socket.hasSelection)
            {
                var oggettoDaDistruggere = socket.GetOldestInteractableSelected();
                socket.interactionManager.SelectCancel(socket, oggettoDaDistruggere);
                Destroy(oggettoDaDistruggere.transform.gameObject);
            }
        }
    }

    private void SpawnResultCard(string nomeResult)
    {
        if (spawnSocket.hasSelection)
        {
            Destroy(spawnSocket
                .GetOldestInteractableSelected()
                .transform
                .gameObject);
        }

        GameObject card = Instantiate(
            resultCardPrefab,
            spawnResultCard.position,
            spawnResultCard.rotation
        );

        card.SetActive(true);
        eliminaOggettiInSockets();

        DataCard resultCard = card.GetComponent<DataCard>();
        if (resultCard != null)
        {
            resultCard.SetName(nomeResult);
            resultCard.SetValue(result);
            resultCard.SetUnitaMisura(unitaMisura);
            resultCard.SetTextTarghetta();
        }
        audioSource.PlayOneShot(clickSoundSuccess);
        changePageScript.ChangePageNext();

    }

    private bool ConversioneSocket(XRSocketInteractor[] sockets)
    {
        if (sockets != null)
        {
            value = new Dictionary<string, float>();
        }
        foreach (XRSocketInteractor socket in sockets) {
            if (socket.hasSelection)
            {
                DataCard data = socket.GetOldestInteractableSelected().transform.GetComponent<DataCard>();

                if (data != null)
                {
                    value.Add(data.GetName(), data.GetValue());
                }
            }
            else{
                Debug.Log("Socket " + socket.name + " is empty. Please fill all sockets.");
                audioSource.PlayOneShot(clickSoundError);
                return false;
            }
        }
        return true;
    }

    private bool AltezzaSeno()
    {
        if(value.ContainsKey("Lunghezza") && value.ContainsKey("Angolo")) 
        {
            result = value["Lunghezza"] * Mathf.Sin(value["Angolo"] * Mathf.Deg2Rad);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for AltezzaSeno calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool AltezzaCoseno()
    {
        if (value.ContainsKey("Lunghezza") && value.ContainsKey("Angolo"))
        {
            result = value["Lunghezza"] * Mathf.Cos(value["Angolo"] * Mathf.Deg2Rad);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for AltezzaCoseno calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool AltezzaTan()
    {
        if(value.ContainsKey("Altezza") && value.ContainsKey("Angolo")) 
        {
            result = value["Altezza"] * Mathf.Tan(value["Angolo"] * Mathf.Deg2Rad);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for AltezzaTan calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool MassaDaInerzia()
    {
        if(value.ContainsKey("Inerzia") && value.ContainsKey("Velocita") && value.ContainsKey("Accelerazione di gravita") && value.ContainsKey("Altezza") && value.ContainsKey("Raggio")) 
        {
            result = value["Inerzia"] * Mathf.Pow(value["Velocita"], 2) / (Mathf.Pow(value["Raggio"], 2) * ((2 * value["Accelerazione di gravita"] * value["Altezza"]) - Mathf.Pow(value["Velocita"],2)));
            return true;
        }
        else
        {
            Debug.Log("Missing required values for MassaDaInerzia calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }
    private bool CostanteInerzialeX1()
    {
        float localRes;
        if(value.ContainsKey("Accelerazione di gravita") && value.ContainsKey("Altezza") && value.ContainsKey("Velocita"))
        {
            localRes = ((value["Accelerazione di gravita"] * value["Altezza"]) / Mathf.Pow(value["Velocita"], 2)) - 1;
            result = Mathf.Round(localRes * 10f) / 10f;
            return true;
        }
        else
        {
            Debug.Log("Missing required values for CostanteInerzialeX1 calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool CostanteInerzialeX2()
    {
        float localRes;
        if (value.ContainsKey("Accelerazione di gravita") && value.ContainsKey("Altezza") && value.ContainsKey("Velocita"))
        {
            localRes = Mathf.Sqrt(((2 * value["Accelerazione di gravita"] * value["Altezza"]) / Mathf.Pow(value["Velocita"], 2)) - 1);
            result = Mathf.Round(localRes * 10f) / 10f;
            return true;
        }
        else
        {
            Debug.Log("Missing required values for CostanteInerzialeX2 calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool CostanteInerzialeO()
    {
        float localRes;
        if (value.ContainsKey("Accelerazione di gravita") && value.ContainsKey("Altezza") && value.ContainsKey("Velocita"))
        {
            localRes = (2*(value["Accelerazione di gravita"] * value["Altezza"]) / Mathf.Pow(value["Velocita"], 2)) - 1;
            result = Mathf.Round(localRes * 10f) / 10f;
            return true;
        }
        else
        {
            Debug.Log("Missing required values for CostanteInerzialeO calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private void Calcolo(Operazione operazione)
    {
        bool esito = false;
        if (!ConversioneSocket(sockets))
        {
            audioSource.PlayOneShot(clickSoundError);
            return;
        }
        else
        {
            switch (operazione)
            {
                case Operazione.AltezzaSeno:
                    esito = AltezzaSeno();
                    unitaMisura = simboliUnita[UnitaMisura.Metri];
                    if (esito) SpawnResultCard("Altezza");
                    Debug.Log("Altezza: " + result);
                    break;
                case Operazione.AltezzaCoseno:
                    esito = AltezzaCoseno();
                    unitaMisura = simboliUnita[UnitaMisura.Metri];
                    if (esito) SpawnResultCard("Altezza");
                    Debug.Log("Altezza: " + result);
                    break;
                case Operazione.AltezzaTan:
                    esito = AltezzaTan();
                    unitaMisura = simboliUnita[UnitaMisura.Metri];
                    if (esito) SpawnResultCard("Altezza");
                    Debug.Log("Altezza: " + result);
                    break;
                case Operazione.MassaDaInerzia:
                    esito = MassaDaInerzia();
                    unitaMisura = simboliUnita[UnitaMisura.Kilogrammi];
                    if (esito) SpawnResultCard("Massa");
                    break;
                case Operazione.costanteInerzialeX1:
                    esito = CostanteInerzialeX1();
                    unitaMisura = simboliUnita[UnitaMisura.None];
                    if (esito) SpawnResultCard("Costante Inerziale");
                    break;
                case Operazione.costanteInerzialeX2:
                    esito = CostanteInerzialeX2();
                    unitaMisura = simboliUnita[UnitaMisura.None];
                    if (esito) SpawnResultCard("Costante Inerziale");
                    break;
                case Operazione.costanteInerzialeO:
                    esito = CostanteInerzialeO();
                    unitaMisura = simboliUnita[UnitaMisura.None];
                    if (esito) SpawnResultCard("Costante Inerziale");
                    break;


            }
        }   
    }

        public void CalcoloAltezzaSeno() => Calcolo(Operazione.AltezzaSeno);
        public void CalcoloAltezzaCoseno() => Calcolo(Operazione.AltezzaCoseno);
        public void CalcoloAltezzaTan() => Calcolo(Operazione.AltezzaTan);
        public void CalcoloMassaDaInerzia() => Calcolo(Operazione.MassaDaInerzia);
        public void CalcoloCostanteInerzialeX1() => Calcolo(Operazione.costanteInerzialeX1);
        public void CalcoloCostanteInerzialeX2() => Calcolo(Operazione.costanteInerzialeX2);
        public void CalcoloCostanteInerzialeO() => Calcolo(Operazione.costanteInerzialeO);

}

