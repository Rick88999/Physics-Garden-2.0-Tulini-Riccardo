using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class CestaCarrucola : MonoBehaviour
{
    [SerializeField] private Material materialeDesiderato;
    [SerializeField] private float altezza = 5.6f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;
    private XRSocketInteractor socketInteractor;
    private float energiaPotenziale;
    private void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(InBasket);
        socketInteractor.selectExited.AddListener(OutBasket);
    }

    private void InBasket(SelectEnterEventArgs args)
    {
        GameObject go = args.interactableObject.transform.gameObject;
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        if (go.GetComponent<BoxCollider>() != null && mr != null)
        {
            if (mr.material == materialeDesiderato)
                audioSource.PlayOneShot(successSound);
            else
            {
                audioSource.PlayOneShot(errorSound);

                CalcoloEnergiaPotenziale(go.GetComponent<Rigidbody>().mass);
                Destroy(go);
            }
        }
        else
        {
            audioSource.PlayOneShot(errorSound);
            Destroy(go);
        }
    }

    private void OutBasket(SelectExitEventArgs args) => energiaPotenziale = 0f;
    
    private void CalcoloEnergiaPotenziale(float massa) => energiaPotenziale = massa * 9.81f * altezza;

    public float GetEnergiaPotenziale() => energiaPotenziale;
    
}
