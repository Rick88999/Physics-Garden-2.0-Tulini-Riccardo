using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CalculationCore : MonoBehaviour
{
    private Dictionary<UnitaMisura, string> simboliUnita = new Dictionary<UnitaMisura, string>()
    {
        { UnitaMisura.Metri,       "m"  },
        { UnitaMisura.Centimetri,  "cm" },
        { UnitaMisura.Millimetri,  "mm" },
        { UnitaMisura.Velocita,    "m/s" }
    };


    [SerializeField]XRSocketInteractor[] sockets;
    [SerializeField] XRSocketInteractor spawnSocket;
    [SerializeField] GameObject resultCardPrefab;
    [SerializeField] Transform spawnResultCard;
    [SerializeField] changePage changePageScript;
    private float[] value;
    private float result;
    
    private string unitaMisura;

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
        changePageScript.ChangePageNext();
    }

    private bool ConversioneSocket(XRSocketInteractor[] sockets)
    {
        if (sockets != null)
        {
            value = new float[sockets.Length];
        }
        foreach (XRSocketInteractor socket in sockets) {
            if (socket.hasSelection)
            {
                DataCard data = socket.GetOldestInteractableSelected().transform.GetComponent<DataCard>();

                if (data != null)
                {
                    value[System.Array.IndexOf(sockets, socket)] = data.GetValue();
                }
            }
            else{
                Debug.LogError("Socket " + socket.name + " is empty. Please fill all sockets.");
                return false;
            }
        }
        return true;
    }

    private void AltezzaSeno()
    {
        result = value[0] * Mathf.Sin(value[1] * Mathf.Deg2Rad);
    }

    private void AltezzaCoseno()
    {
        result = value[0] * Mathf.Cos(value[1] * Mathf.Deg2Rad);
    }

    private void AltezzaTan()
    {
        result = value[0] * Mathf.Tan(value[1] * Mathf.Deg2Rad);
    }

    private void Calcolo(Operazione operazione)
    {
        if (!ConversioneSocket(sockets))
        {
            return;
        }
        else
        {
            switch (operazione)
            {
                case Operazione.AltezzaSeno:
                    AltezzaSeno();
                    unitaMisura = simboliUnita[UnitaMisura.Metri];
                    SpawnResultCard("Altezza h");
                    Debug.Log("Altezza: " + result);
                    break;
                case Operazione.AltezzaCoseno:
                    AltezzaCoseno();
                    unitaMisura = simboliUnita[UnitaMisura.Metri];
                    SpawnResultCard("Altezza h");
                    Debug.Log("Altezza: " + result);
                    break;
                case Operazione.AltezzaTan:
                    AltezzaTan();
                    unitaMisura = simboliUnita[UnitaMisura.Metri];
                    SpawnResultCard("Altezza h");
                    Debug.Log("Altezza: " + result);
                    break;

            }
        }   
    }

        public void CalcoloAltezzaSeno() => Calcolo(Operazione.AltezzaSeno);
        public void CalcoloAltezzaCoseno() => Calcolo(Operazione.AltezzaCoseno);
        public void CalcoloAltezzaTan() => Calcolo(Operazione.AltezzaTan);
}

