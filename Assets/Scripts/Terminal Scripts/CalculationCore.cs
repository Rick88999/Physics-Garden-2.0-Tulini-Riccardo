using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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
        { UnitaMisura.DensitaX1, "m<sup>3</sup>/kg" },
        { UnitaMisura.DensitaO, "kg/m<sup>3</sup>" },
        { UnitaMisura.Volume, "m<sup>3</sup>" },
        { UnitaMisura.VolumeX1, "m<sup>2</sup>" },
        { UnitaMisura.Inerzia, "kg*m<sup>2</sup>" },
        { UnitaMisura.InerziaX2, "kg*m<sup>3</sup>" },
        { UnitaMisura.InerziaX3, "kg<sup>2</sup>*m" },
        { UnitaMisura.Newton, "N" },
        { UnitaMisura.Jouole, "J" },
        { UnitaMisura.None, "" }
    };
    [SerializeField] private GameManager gm;
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

    private bool EnergiaCinetica()
    {
        if((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Velocita") || value.ContainsKey("Velocity"))) 
        {
            result = 0.5f * (value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]) * Mathf.Pow((value.ContainsKey("Velocita") ? value["Velocita"] : value["Velocity"]), 2);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for EnegiaCinetica calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool EnergiaPotenziale()
    {
        if((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Accelerazione di gravita") || value.ContainsKey("Gravity Acceleration")) && (value.ContainsKey("Altezza") || value.ContainsKey("Height"))) 
        {
            result = (value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]) * (value.ContainsKey("Accelerazione di gravita") ? value["Accelerazione di gravita"] : value["Gravity Acceleration"]) * (value.ContainsKey("Altezza") ? value["Altezza"] : value["Height"]);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for EnergiaPotenziale calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool EnergiaElastica()
    {
        if((value.ContainsKey("Costante elastica") || value.ContainsKey("Elastic constant")) && (value.ContainsKey("Spostamento") || value.ContainsKey("Displacement"))) 
        {
            result = 0.5f * (value.ContainsKey("Costante elastica") ? value["Costante elastica"] : value["Elastic constant"]) * Mathf.Pow((value.ContainsKey("Spostamento") ? value["Spostamento"] : value["Displacement"]), 2);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for EnergiaElastica calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool ForzaElastica()
    {
        if((value.ContainsKey("Costante elastica") || value.ContainsKey("Elastic constant")) && (value.ContainsKey("Spostamento") || value.ContainsKey("Displacement"))) 
        {
            result = value.ContainsKey("Costante elastica") ? value["Costante elastica"] : value["Elastic constant"];
            result *= value.ContainsKey("Spostamento") ? value["Spostamento"] : value["Displacement"];
            return true;
        }
        else
        {
            Debug.Log("Missing required values for ForzaElastica calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool ForzaNewtoniana()
    {
        if ((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && value.Keys.Any(k => k.Contains("Accelerazione") || k.Contains("Acceleration")))
        {
            string chiaveAccelerazione = value.Keys.First(k => k.Contains("Accelerazione") || k.Contains("Acceleration"));
            result = value.ContainsKey("Massa") ? value["Massa"] : value["Mass"];
            result *= value[chiaveAccelerazione];
            return true;
        }
        else
        {
            Debug.Log("Missing required values for ForzaNewtoniana calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool ForzaCentripeta()
    {
        if ((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Velocita") || value.ContainsKey("Velocity")) && (value.ContainsKey("Raggio") || value.ContainsKey("Radius")))
        {
            result = (value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]) * Mathf.Pow(value.ContainsKey("Velocita") ? value["Velocita"] : value["Velocity"], 2) / (value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"]);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for ForzaCentripeta calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }
    private bool ForzaAttritoStatico()
    {
        if ((value.ContainsKey("Coefficiente di attrito statico") || value.ContainsKey("Static friction coefficient")) && (value.ContainsKey("Reazione vincolare") || value.ContainsKey("Normal force")))
        {
            result = (value.ContainsKey("Coefficiente di attrito statico") ? value["Coefficiente di attrito statico"] : value["Static friction coefficient"]) * (value.ContainsKey("Reazione vincolare") ? value["Reazione vincolare"] : value["Normal force"]);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for ForzaAttritoStatico calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool ForzaElettrostatica()
    {
        if ((value.ContainsKey("Carica") || value.ContainsKey("Charge")) && (value.ContainsKey("Campo Elettrico") || value.ContainsKey("Electric field")))
        {
            result = (value.ContainsKey("Carica") ? value["Carica"] : value["Charge"]) * (value.ContainsKey("Campo Elettrico") ? value["Campo Elettrico"] : value["Electric field"]);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for ForzaElettrostatica calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool ForzaDiLorentz()
    {
        if ((value.ContainsKey("Carica") || value.ContainsKey("Charge")) && (value.ContainsKey("Campo Magnetico") || value.ContainsKey("Magnetic field")) && (value.ContainsKey("Velocita") || value.ContainsKey("Velocity")) && (value.ContainsKey("Angolo") || value.ContainsKey("Angle")))
        {
            result = (value.ContainsKey("Carica") ? value["Carica"] : value["Charge"]) * (value.ContainsKey("Campo Magnetico") ? value["Campo Magnetico"] : value["Magnetic field"]) * (value.ContainsKey("Velocita") ? value["Velocita"] : value["Velocity"]) * Mathf.Sin(value.ContainsKey("Angolo") ? value["Angolo"] : value["Angle"]);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for ForzaDiLorentz calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool VolumeCubo()
    {
        if(value.ContainsKey("Lato") || value.ContainsKey("Side")) 
        {
            result = Mathf.Pow(value.ContainsKey("Lato") ? value["Lato"] : value["Side"], 3);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for VolumeCubo calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool VolumeSfera()
    {
        if (value.ContainsKey("Raggio") || value.ContainsKey("Radius"))
        {
            result = (4f / 3f) * Mathf.PI * Mathf.Pow(value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"], 3);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for VolumeSfera calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool VolumeCilindro()
    {
        if ((value.ContainsKey("Raggio") || value.ContainsKey("Radius")) && (value.ContainsKey("Altezza") || value.ContainsKey("Height")))
        {
            result = Mathf.PI * Mathf.Pow(value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"], 2) * (value.ContainsKey("Altezza") ? value["Altezza"] : value["Height"]);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for VolumeCilindro calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool VolumeX1()
    {
        if (value.ContainsKey("Raggio") || value.ContainsKey("Radius"))
        {
            result = (1f / 3f) * Mathf.PI * Mathf.Pow(value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"], 2);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for VolumeX1 calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }
    private bool DensitaX1()
    {
        if((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Volume") || value.ContainsKey("Volume"))) 
        {
            result = (value.ContainsKey("Volume") ? value["Volume"] : value["Volume"]) / (value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for DensitaX1 calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool DensitaArch()
    {
        if ((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Accelerazione di gravita") || value.ContainsKey("Gravity Acceleration")) && (value.ContainsKey("Densita fluido") || value.ContainsKey("Fluid density")) && value.Keys.Any(k => k.Contains("Forza") || k.Contains("Force")))
        {
            string chiaveForza = value.Keys.First(k => k.Contains("Forza") || k.Contains("Force"));
            result = (value.ContainsKey("Densita fluido") ? value["Densita fluido"] : value["Fluid density"]) * (value[chiaveForza] / ((value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]) * (value.ContainsKey("Accelerazione di gravita") ? value["Accelerazione di gravita"] : value["Gravity Acceleration"])));
            return true;
        }
        else
        {
            Debug.Log("Missing required values for DensitaArch calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool DensitaO()
    {
        if ((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Volume") || value.ContainsKey("Volume")))
        {
            result = (value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]) / (value.ContainsKey("Volume") ? value["Volume"] : value["Volume"]);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for DensitaX1 calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }
    private bool AltezzaSeno()
    {
        if((value.ContainsKey("Lunghezza") || value.ContainsKey("Length")) && (value.ContainsKey("Angolo") || value.ContainsKey("Angle"))) 
        {
            result = (value.ContainsKey("Lunghezza") ? value["Lunghezza"] : value["Length"]) * Mathf.Sin((value.ContainsKey("Angolo") ? value["Angolo"] : value["Angle"]) * Mathf.Deg2Rad);
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
        if ((value.ContainsKey("Lunghezza") || value.ContainsKey("Length")) && (value.ContainsKey("Angolo") || value.ContainsKey("Angle")))
        {
            result = (value.ContainsKey("Lunghezza") ? value["Lunghezza"] : value["Length"]) * Mathf.Cos((value.ContainsKey("Angolo") ? value["Angolo"] : value["Angle"]) * Mathf.Deg2Rad);
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
        if((value.ContainsKey("Lunghezza") || value.ContainsKey("Length")) && (value.ContainsKey("Angolo") || value.ContainsKey("Angle"))) 
        {
            result = (value.ContainsKey("Lunghezza") ? value["Lunghezza"] : value["Length"]) * Mathf.Tan((value.ContainsKey("Angolo") ? value["Angolo"] : value["Angle"]) * Mathf.Deg2Rad);
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
        if((value.ContainsKey("Inerzia") || value.ContainsKey("Inertia")) && (value.ContainsKey("Velocita") || value.ContainsKey("Velocity")) && (value.ContainsKey("Accelerazione di gravita") || value.ContainsKey("Gravity Acceleration")) && (value.ContainsKey("Altezza") || value.ContainsKey("Height")) && (value.ContainsKey("Raggio") || value.ContainsKey("Radius"))) 
        {
            result = (value.ContainsKey("Inerzia") ? value["Inerzia"] : value["Inertia"]) * Mathf.Pow((value.ContainsKey("Velocita") ? value["Velocita"] : value["Velocity"]), 2) / (Mathf.Pow((value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"]), 2) * ((2 * (value.ContainsKey("Accelerazione di gravita") ? value["Accelerazione di gravita"] : value["Gravity Acceleration"]) * (value.ContainsKey("Altezza") ? value["Altezza"] : value["Height"])) - Mathf.Pow((value.ContainsKey("Velocita") ? value["Velocita"] : value["Velocity"]),2)));
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
        if((value.ContainsKey("Accelerazione di gravita") || value.ContainsKey("Gravity Acceleration")) && (value.ContainsKey("Altezza") || value.ContainsKey("Height")) && (value.ContainsKey("Velocita") || value.ContainsKey("Velocity")))
        {
            localRes = ((value.ContainsKey("Accelerazione di gravita") ? value["Accelerazione di gravita"] : value["Gravity Acceleration"]) * (value.ContainsKey("Altezza") ? value["Altezza"] : value["Height"])) / Mathf.Pow((value.ContainsKey("Velocita") ? value["Velocita"] : value["Velocity"]), 2) - 1;
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
        if ((value.ContainsKey("Accelerazione di gravita") || value.ContainsKey("Gravity Acceleration")) && (value.ContainsKey("Altezza") || value.ContainsKey("Height")) && (value.ContainsKey("Velocita") || value.ContainsKey("Velocity")))
        {
            localRes = Mathf.Sqrt(((2 * (value.ContainsKey("Accelerazione di gravita") ? value["Accelerazione di gravita"] : value["Gravity Acceleration"]) * (value.ContainsKey("Altezza") ? value["Altezza"] : value["Height"])) / Mathf.Pow((value.ContainsKey("Velocita") ? value["Velocita"] : value["Velocity"]), 2)) - 1);
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
        if ((value.ContainsKey("Accelerazione di gravita") || value.ContainsKey("Gravity Acceleration")) && (value.ContainsKey("Altezza") || value.ContainsKey("Height")) && (value.ContainsKey("Velocita") || value.ContainsKey("Velocity")))
        {
            localRes = (2 * ((value.ContainsKey("Accelerazione di gravita") ? value["Accelerazione di gravita"] : value["Gravity Acceleration"]) * (value.ContainsKey("Altezza") ? value["Altezza"] : value["Height"])) / Mathf.Pow((value.ContainsKey("Velocita") ? value["Velocita"] : value["Velocity"]), 2)) - 1;
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

    private bool InerziaX1()
    {
        if((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Raggio") || value.ContainsKey("Radius")))
        {
            result = (value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]) * Mathf.Pow((value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"]), 2);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for InerziaX1 calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool InerziaX2()
    {
        if ((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Raggio") || value.ContainsKey("Radius")) && (value.ContainsKey("Costante Inerziale") || value.ContainsKey("Inertia Constant")))
        {
            result = (value.ContainsKey("Costante Inerziale") ? value["Costante Inerziale"] : value["Inertia Constant"]) * (value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]) * Mathf.Pow((value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"]), 3);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for InerziaX2 calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool InerziaX3()
    {
        if ((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Raggio") || value.ContainsKey("Radius")) && (value.ContainsKey("Costante Inerziale") || value.ContainsKey("Inertia Constant")))
        {
            result = (value.ContainsKey("Costante Inerziale") ? value["Costante Inerziale"] : value["Inertia Constant"]) * (value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"]) * Mathf.Pow((value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]), 2);
            return true;
        }
        else
        {
            Debug.Log("Missing required values for InerziaX3 calculation.");
            audioSource.PlayOneShot(clickSoundError);
            return false;
        }
    }

    private bool InerziaO()
    {
        if ((value.ContainsKey("Massa") || value.ContainsKey("Mass")) && (value.ContainsKey("Raggio") || value.ContainsKey("Radius")) && (value.ContainsKey("Costante Inerziale") || value.ContainsKey("Inertia Constant")))
        {
            result = (value.ContainsKey("Costante Inerziale") ? value["Costante Inerziale"] : value["Inertia Constant"]) * (value.ContainsKey("Massa") ? value["Massa"] : value["Mass"]) * Mathf.Pow((value.ContainsKey("Raggio") ? value["Raggio"] : value["Radius"]), 2);
            result = Mathf.Round(result * 1000f) / 1000f;
            return true;
        }
        else
        {
            Debug.Log("Missing required values for InerziaO calculation.");
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
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Altezza");
                        else if(gm.GetLanguage() == Language.English)
                            SpawnResultCard("Height");
                    }
                    Debug.Log("Altezza: " + result);
                    break;
                case Operazione.AltezzaCoseno:
                    esito = AltezzaCoseno();
                    unitaMisura = simboliUnita[UnitaMisura.Metri];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Altezza");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Height");
                    }
                    Debug.Log("Altezza: " + result);
                    break;
                case Operazione.AltezzaTan:
                    esito = AltezzaTan();
                    unitaMisura = simboliUnita[UnitaMisura.Metri];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Altezza");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Height");
                    }
                    Debug.Log("Altezza: " + result);
                    break;
                case Operazione.MassaDaInerzia:
                    esito = MassaDaInerzia();
                    unitaMisura = simboliUnita[UnitaMisura.Kilogrammi];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Massa");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Mass");
                    }
                    break;
                case Operazione.costanteInerzialeX1:
                    esito = CostanteInerzialeX1();
                    unitaMisura = simboliUnita[UnitaMisura.None];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Costante Inerziale");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Inertia Constant");
                    }
                    break;
                case Operazione.costanteInerzialeX2:
                    esito = CostanteInerzialeX2();
                    unitaMisura = simboliUnita[UnitaMisura.None];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Costante Inerziale");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Inertia Constant");
                    }   
                    break;
                case Operazione.costanteInerzialeO:
                    esito = CostanteInerzialeO();
                    unitaMisura = simboliUnita[UnitaMisura.None];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Costante Inerziale");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Inertia Constant");
                    }
                    break;
                case Operazione.InerziaX1:
                    esito = InerziaX1();
                    unitaMisura = simboliUnita[UnitaMisura.Inerzia];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Inerzia");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Inertia");
                    }
                    break;
                case Operazione.InerziaX2:
                    esito = InerziaX2();
                    unitaMisura = simboliUnita[UnitaMisura.InerziaX2];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Inerzia");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Inertia");
                    }
                    break;
                case Operazione.InerziaX3:
                    esito = InerziaX3();
                    unitaMisura = simboliUnita[UnitaMisura.InerziaX3];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Inerzia");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Inertia");
                    }
                    break;
                case Operazione.InerziaO:
                    esito = InerziaO();
                    unitaMisura = simboliUnita[UnitaMisura.Inerzia];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Inerzia");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Inertia");
                    }
                    break;
                case Operazione.DensitaX1:
                    esito = DensitaX1();
                    unitaMisura = simboliUnita[UnitaMisura.DensitaX1];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Densità");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Density");
                    }
                    break;
                case Operazione.DensitaArch:
                    esito = DensitaArch();
                    unitaMisura = simboliUnita[UnitaMisura.DensitaO];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Densità");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Density");
                    }
                    break;
                case Operazione.DensitaO:
                    esito = DensitaO();
                    unitaMisura = simboliUnita[UnitaMisura.DensitaO];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Densità");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Density");
                    }
                    break;
                case Operazione.VolumeCubo:
                    esito = VolumeCubo();
                    unitaMisura = simboliUnita[UnitaMisura.Volume];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Volume");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Volume");
                    }
                    break;
                case Operazione.VolumeSfera:
                    esito = VolumeSfera();
                    unitaMisura = simboliUnita[UnitaMisura.Volume];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Volume");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Volume");
                    }
                    break;
                case Operazione.VolumeCilindro:
                    esito = VolumeCilindro();
                    unitaMisura = simboliUnita[UnitaMisura.Volume];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Volume");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Volume");
                    }
                    break;
                case Operazione.VolumeX1:
                    esito = VolumeX1();
                    unitaMisura = simboliUnita[UnitaMisura.VolumeX1];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("VolumeX1");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("VolumeX1");
                    }
                    break;
                case Operazione.ForzaCentripeta:
                    esito = ForzaCentripeta();
                    unitaMisura = simboliUnita[UnitaMisura.Newton];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Forza Centripeta");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Centripetal Force");
                    }
                    break;
                case Operazione.ForzaNewtoniana:
                    esito = ForzaNewtoniana();
                    unitaMisura = simboliUnita[UnitaMisura.Newton];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Forza Newtoniana");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Newtonian Force");
                    }
                    break;
                case Operazione.ForzaElastica:
                    esito = ForzaElastica();
                    unitaMisura = simboliUnita[UnitaMisura.Newton];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Forza Elastica");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Elastic Force");
                    }
                    break;
                case Operazione.ForzaElettrostatica:
                    esito = ForzaElettrostatica();
                    unitaMisura = simboliUnita[UnitaMisura.Newton];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Forza Elettrostatica");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Electrostatic Force");
                    }
                    break;
                case Operazione.ForzaDiLorentz:
                    esito = ForzaDiLorentz();
                    unitaMisura = simboliUnita[UnitaMisura.Newton];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Forza di Lorentz");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Lorentz Force");
                    }
                    break;
                case Operazione.ForzaAttritoStatico:
                    esito = ForzaAttritoStatico();
                    unitaMisura = simboliUnita[UnitaMisura.Newton];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Forza di Attrito Statico");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Static Friction Force");
                    }
                    break;
                case Operazione.EnergiaCinetica:
                    esito = EnergiaCinetica();
                    unitaMisura = simboliUnita[UnitaMisura.Jouole];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Energia Cinetica");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Kinetic Energy");
                    }
                    break;
                case Operazione.EnergiaPotenziale:
                    esito = EnergiaPotenziale();
                    unitaMisura = simboliUnita[UnitaMisura.Jouole];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Energia Potenziale");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Potential Energy");
                    }
                    break;
                case Operazione.EnergiaElastica:
                    esito = EnergiaElastica();
                    unitaMisura = simboliUnita[UnitaMisura.Jouole];
                    if (esito)
                    {
                        if (gm.GetLanguage() == Language.Italian)
                            SpawnResultCard("Energia Elastica");
                        else if (gm.GetLanguage() == Language.English)
                            SpawnResultCard("Elastic Energy");
                    }
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
        public void CalcoloInerziaX1() => Calcolo(Operazione.InerziaX1);
        public void CalcoloInerziaX2() => Calcolo(Operazione.InerziaX2);
        public void CalcoloInerziaX3() => Calcolo(Operazione.InerziaX3);
        public void CalcoloInerziaO() => Calcolo(Operazione.InerziaO);
        public void CalcoloDensitaX1() => Calcolo(Operazione.DensitaX1);
        public void CalcoloDensitaArch() => Calcolo(Operazione.DensitaArch);
        public void CalcoloDensitaO() => Calcolo(Operazione.DensitaO);
        public void CalcoloVolumeCubo() => Calcolo(Operazione.VolumeCubo);
        public void CalcoloVolumeSfera() => Calcolo(Operazione.VolumeSfera);
        public void CalcoloVolumeCilindro() => Calcolo(Operazione.VolumeCilindro);
        public void CalcoloVolumeX1() => Calcolo(Operazione.VolumeX1);
        public void CalcoloForzaCentripeta() => Calcolo(Operazione.ForzaCentripeta);
        public void CalcoloForzaNewtoniana() => Calcolo(Operazione.ForzaNewtoniana);
        public void CalcoloForzaElastica() => Calcolo(Operazione.ForzaElastica);
        public void CalcoloForzaElettrostatica() => Calcolo(Operazione.ForzaElettrostatica);
        public void CalcoloForzaDiLorentz() => Calcolo(Operazione.ForzaDiLorentz);
        public void CalcoloForzaAttritoStatico() => Calcolo(Operazione.ForzaAttritoStatico);
        public void CalcoloEnergiaCinetica() => Calcolo(Operazione.EnergiaCinetica);
        public void CalcoloEnergiaPotenziale() => Calcolo(Operazione.EnergiaPotenziale);
        public void CalcoloEnergiaElastica() => Calcolo(Operazione.EnergiaElastica);
}

