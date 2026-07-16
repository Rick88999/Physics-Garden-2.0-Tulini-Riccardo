using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SpawnOggettoPuroRotolamento : MonoBehaviour
{
    [SerializeField] private GameObject oggettoPrefab;
    private GameObject oggettoInSocket;
    private GameObject lastObjectInSocket;
    private XRSocketInteractor socket;
    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnObjectInSocket);
        socket.selectExited.AddListener(OnObjectRemovedFromSocket);
        SpawnOggettoPianoInclinato();
    }

    public void SpawnOggettoPianoInclinato()
    {
        if (oggettoPrefab != null && socket != null)
        {  
            if(!socket.hasSelection)
            {
                oggettoInSocket = Instantiate(oggettoPrefab, oggettoPrefab.transform.position, oggettoPrefab.transform.rotation);
                oggettoInSocket.SetActive(true);


                if(lastObjectInSocket != null) Destroy(lastObjectInSocket);
                
                lastObjectInSocket = oggettoInSocket; 
            }
            
        }  
    }

    private void OnObjectInSocket(SelectEnterEventArgs args)
    {
        oggettoInSocket = args.interactableObject.transform.gameObject;
        lastObjectInSocket = args.interactableObject.transform.gameObject;
    }

    private void OnObjectRemovedFromSocket(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.gameObject == lastObjectInSocket)
        {
            oggettoInSocket = null;
        }
    }
}
