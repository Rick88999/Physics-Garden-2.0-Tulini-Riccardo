using System.Collections;
using UnityEditor;
using UnityEngine;

public class CestaCarrucola : MonoBehaviour
{
    [SerializeField] private Material materialeDesiderato;
    [SerializeField] private float altezza = 5.6f;
    [SerializeField] private float fattoreDiScala = 1.0f;
    [SerializeField] private GameObject warningCanvas;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;
    private float energiaPotenziale;
    private void OnTriggerEnter(Collider other) 
    {
        GameObject go = other.gameObject;
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        float SizeM = Mathf.Max(go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
        if (go.GetComponent<BoxCollider>() != null && mr != null)
        {
            if (mr.sharedMaterial == materialeDesiderato)
            {
                if(SizeM >= 3*fattoreDiScala)
                {
                    audioSource.PlayOneShot(successSound);
                    CalcoloEnergiaPotenziale(go.GetComponent<Rigidbody>().mass);
                }
                else WrongObjectType(go);
            }
            else WrongObjectType(go);
        }
        else WrongObjectType(go);
        
    }

    private void WrongObjectType(GameObject go)
    {
        StartCoroutine(ShowWarningCanvas(4f));
        audioSource.PlayOneShot(errorSound);
        Destroy(go);
    }

    private void OnTriggerExit(Collider other)
    {
        energiaPotenziale -= other.gameObject.GetComponent<Rigidbody>().mass * 9.81f * altezza;
        if (energiaPotenziale < 0f)
        {
            energiaPotenziale = 0f;
        }
    }

    public IEnumerator ShowWarningCanvas(float duration)
    {
        if (warningCanvas != null)
        {
            warningCanvas.SetActive(true);
            yield return new WaitForSeconds(duration);
            warningCanvas.SetActive(false);
        }
    }

    private void CalcoloEnergiaPotenziale(float massa) => energiaPotenziale += massa * 9.81f * altezza;

    public float GetEnergiaPotenziale() => energiaPotenziale;  
}
