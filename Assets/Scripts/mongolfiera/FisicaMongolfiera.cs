using UnityEngine;

public class FisicaMongolfiera : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Parametri Fisici")]
    [SerializeField] private float volumeOggetto;//10f
    [SerializeField] private float densitaFluido; //1f o 1.2f
    [SerializeField] private float altezzaMassima;//8f

    [Header("Controllo Altezza")]
    [Tooltip("Forza del freno quando ci si avvicina all'altezza massima")]
    [SerializeField] private float intensitaFrenata = 2f; 

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        // resistenza aria (anti effetto yo yo)
        rb.linearDamping = 1.5f; 
    }

    void FixedUpdate()
    {
        float gravita = Mathf.Abs(Physics.gravity.y);
        
        
        float spintaArchimede = volumeOggetto * densitaFluido * gravita;

        
        Vector3 forzaSpinta = Vector3.up * spintaArchimede;

        if (transform.position.y > altezzaMassima)
        {
            
            float eccessoAltezza = transform.position.y - altezzaMassima;
            forzaSpinta += Vector3.down * (eccessoAltezza * intensitaFrenata * gravita * rb.mass);
        }
        else if (transform.position.y > altezzaMassima - 2f)
        {
            float fattoreRallentamento = (transform.position.y - (altezzaMassima - 2f)) / 2f;
            rb.linearDamping = Mathf.Lerp(1.5f, 5f, fattoreRallentamento);
        }
        else
        {
            rb.linearDamping = 1.5f;
        }

        rb.AddForce(forzaSpinta, ForceMode.Force);
    }
}
