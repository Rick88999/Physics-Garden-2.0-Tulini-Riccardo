using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SpawnCard : MonoBehaviour
{
    private Dictionary<UnitaMisura, string> simboliUnita = new Dictionary<UnitaMisura, string>()
    {
        { UnitaMisura.Metri,       "m"  },
        { UnitaMisura.Centimetri,  "cm" },
        { UnitaMisura.Millimetri,  "mm" },
        { UnitaMisura.Velocita,    "m/s" },
        { UnitaMisura.Angolo,      "°"  }
    };

    [SerializeField] public UnitaMisura tipoDataCard;
    [SerializeField] public GameObject dataCardPrefab;
    [SerializeField] public XRSocketInteractor spawnSocket;
    [SerializeField] public Transform spawnResultCard;

    [SerializeField] private float valore;
    private bool valueIsSet = false;
    [SerializeField] private string nome;
    [SerializeField] private string details;

    public void SpawnDataCard()
    {
        if (dataCardPrefab != null )//&& valueIsSet)
        {
            //valueIsSet = false;
            nome = tipoDataCard.ToString();
            string unitaMisura = simboliUnita[tipoDataCard];
            if (spawnSocket.hasSelection)
            {
                Destroy(spawnSocket
                    .GetOldestInteractableSelected()
                    .transform
                    .gameObject);
            }

            GameObject card = Instantiate(
                dataCardPrefab,
                spawnResultCard.position,
                spawnResultCard.rotation
            );

            card.SetActive(true);

            DataCard resultCard = card.GetComponent<DataCard>();
            if (resultCard != null)
            {
                resultCard.SetName(nome);
                resultCard.SetValue(Mathf.Round(valore * 100f) / 100f);
                resultCard.SetUnitaMisura(unitaMisura);
                if (!string.IsNullOrEmpty(details))
                {
                    resultCard.SetDetails(details);
                }
                resultCard.SetTextTarghetta();
            }
        }
        else if(!valueIsSet)
        {
            Debug.LogWarning("Valore non impostato. Impossibile generare la DataCard.");
        }
         else if (dataCardPrefab == null)
        {
            Debug.LogWarning("Prefab della DataCard non assegnato. Impossibile generare la DataCard.");
        }
    }

    public void ValueReceiver(float value)
    {
        valore = value;
        valueIsSet = true;
    }
    public void DetailsReceiver(string details)
    {
        this.details = details;
    }
}
