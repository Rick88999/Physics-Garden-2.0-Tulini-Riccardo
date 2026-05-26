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
        { UnitaMisura.Angolo,      "°"  },
        { UnitaMisura.Kilogrammi,  "kg" }
    };
    
    [SerializeField] public TipoCard tipoCard;
    
    [SerializeField] public GameObject CardPrefab;
    [SerializeField] public XRSocketInteractor spawnSocket;
    [SerializeField] public Transform spawnResultCard;

    [SerializeField] private string nome;

    [Header("Data Card Only")]
    [SerializeField] public UnitaMisura tipoDataCard;
    [SerializeField] private float valore;
    [SerializeField] private string details;
    private bool valueIsSet = false;

    [Header("Formula Card Only")]
    [SerializeField] private GameObject formulaCanvas;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSoundError;
    [SerializeField] private AudioClip clickSoundSuccess;

    [Header("Bot Menu")]
    [SerializeField] private bool isBotMenu = false;

    private GameObject SpawnAction()
    {
        if (!isBotMenu)
        {
            if (spawnSocket.hasSelection)
            {
                Destroy(spawnSocket
                    .GetOldestInteractableSelected()
                    .transform
                    .gameObject);
            }
        }

        if (spawnSocket.hasSelection)
        {
            Destroy(spawnSocket
                .GetOldestInteractableSelected()
                .transform
                .gameObject);
        }

        GameObject card = Instantiate(
            CardPrefab,
            spawnResultCard.position,
            spawnResultCard.rotation
        );

        card.SetActive(true);
        return card;
    }
    public void SpawnFormulaCard()
    {
        if (CardPrefab != null && tipoCard == TipoCard.FormulaCard)
        {
            GameObject card = SpawnAction();
            if (card != null)
            {
                FormulaCard formulaCard = card.GetComponent<FormulaCard>();
                if (formulaCard != null) 
                { 
                    formulaCard.SetName(nome);
                    if (!string.IsNullOrEmpty(details))
                    {
                        formulaCard.SetDetails(details);
                    }
                    formulaCard.SetTextTarghetta();
                }
                if (formulaCanvas != null)
                {
                    FormulaCanvasData formulaCanvasData = card.GetComponent<FormulaCanvasData>();
                    if (formulaCanvasData != null)
                    {
                        formulaCanvasData.SetCanvas(formulaCanvas);
                        audioSource.PlayOneShot(clickSoundSuccess);
                    }
                }
            }
        }
        
    }

    public void SpawnDataCard()
    {
        if (CardPrefab != null && tipoCard == TipoCard.DataCard)
        {
            //valueIsSet = false;
            GameObject card = SpawnAction();
            string unitaMisura = simboliUnita[tipoDataCard];
            DataCard resultCard = card.GetComponent<DataCard>();
            if (resultCard != null)
            {
                resultCard.SetName(nome);
                resultCard.SetValue(valore);
                resultCard.SetUnitaMisura(unitaMisura);
                if (!string.IsNullOrEmpty(details))
                {
                    resultCard.SetDetails(details);
                }
                resultCard.SetTextTarghetta();
                audioSource.PlayOneShot(clickSoundSuccess);
            }
        }
        else if(!valueIsSet)
        {
            Debug.LogWarning("Valore non impostato. Impossibile generare la DataCard.");
            audioSource.PlayOneShot(clickSoundError);
        }
         else if (CardPrefab == null)
        {
            Debug.LogWarning("Prefab della DataCard non assegnato. Impossibile generare la DataCard.");
            audioSource.PlayOneShot(clickSoundError);
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
