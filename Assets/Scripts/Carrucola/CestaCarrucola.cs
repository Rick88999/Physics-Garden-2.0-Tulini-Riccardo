using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class CestaCarrucola : MonoBehaviour
{
    [SerializeField] private Material materialeDesiderato;
    [SerializeField] private float altezza = 5.6f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;
    private float energiaPotenziale;
    private void OnTriggerEnter(Collider other) 
    {
        GameObject go = other.gameObject;
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        if (go.GetComponent<BoxCollider>() != null && mr != null)
        {
            if (mr.sharedMaterial == materialeDesiderato)
            {
                audioSource.PlayOneShot(successSound);
                CalcoloEnergiaPotenziale(go.GetComponent<Rigidbody>().mass);
            }
            else
            {
                audioSource.PlayOneShot(errorSound);
                Destroy(go);
            }
        }
        else
        {
            audioSource.PlayOneShot(errorSound);
            Destroy(go);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        energiaPotenziale -= other.gameObject.GetComponent<Rigidbody>().mass * 9.81f * altezza;
        if (energiaPotenziale < 0f)
        {
            energiaPotenziale = 0f;
        }
    }

    private void CalcoloEnergiaPotenziale(float massa) => energiaPotenziale += massa * 9.81f * altezza;

    public float GetEnergiaPotenziale() => energiaPotenziale;  
}
