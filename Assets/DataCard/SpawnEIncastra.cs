using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SpawnInSocket : MonoBehaviour
{
    public GameObject prefabDaSpawnare;
    public XRSocketInteractor socket;
    public InputActionReference gripInput;

    private bool tastoPremuto = false; // Ci serve per evitare spawn multipli continui

    public void Update()
    {
        float valoreGrip = gripInput.action.ReadValue<float>();

        // Se premiamo il tasto (> 0.1) E non lo stavamo già premendo prima
        if (valoreGrip > 0.1f && !tastoPremuto)
        {
            tastoPremuto = true; // Blocchiamo lo spawn continuo finché non rilasci il tasto

            // Controlliamo che la socket sia vuota prima di spawnare un nuovo oggetto!
            if (!socket.hasSelection)
            {
                StartCoroutine(SpawnaEIncastraRoutine());
            }
            else
            {
                Debug.Log("La socket è già occupata, svuotala prima!");
            }
        }
        // Se rilasciamo il tasto, resettiamo la variabile per poterlo ripremere
        else if (valoreGrip <= 0.1f)
        {
            tastoPremuto = false;
        }
    }

    private IEnumerator SpawnaEIncastraRoutine()
    {
        // 1. Spawna l'oggetto
        GameObject nuovoOggetto = Instantiate(prefabDaSpawnare, socket.transform.position, socket.transform.rotation);
        nuovoOggetto.SetActive(true); // Assicurati che l'oggetto sia attivo (se il prefab è disattivato)

        // 2. Prendi il componente
        IXRSelectInteractable interactable = nuovoOggetto.GetComponent<IXRSelectInteractable>();

        if (interactable != null)
        {
            // 3. ASPETTA 1 FRAME: Risolve l'errore "Interactable is not registered"
            yield return null;

            // 4. Inserisci l'oggetto nella socket
            socket.interactionManager.SelectEnter((IXRSelectInteractor)socket, interactable);
        }
        else
        {
            Debug.LogError("Il prefab non ha il componente XRGrabInteractable!");
        }
    }
}