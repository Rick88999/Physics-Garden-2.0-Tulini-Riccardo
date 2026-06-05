using UnityEngine;

public class FisicaMongolfiera : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Parametri Fisici")]
    public float volumeOggetto = 10f;
    public float densitaFluido = 1.2f;
    public float altezzaMassima = 8f;

    [Header("Controllo Altezza")]
    [Tooltip("Forza del freno quando ci si avvicina all'altezza massima")]
    public float intensitaFrenata = 2f; 

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        // Un buon damping evita che la mongolfiera oscilli all'infinito
        rb.linearDamping = 1.5f; 
    }

    void FixedUpdate()
    {
        float gravita = Mathf.Abs(Physics.gravity.y);
        
        // 1. Calcoliamo la spinta di Archimede pura
        float spintaArchimede = volumeOggetto * densitaFluido * gravita;

        // 2. Applichiamo la spinta verso l'alto (il Rigidbody gestirà già il peso da solo)
        Vector3 forzaSpinta = Vector3.up * spintaArchimede;

        // 3. Sistema di contenimento dell'altezza (Freno software)
        // Se superiamo o ci avviciniamo all'altezza massima, applichiamo una forza contraria
        if (transform.position.y > altezzaMassima)
        {
            // Applica una forza verso il basso proporzionale a quanto lo ha superato
            float eccessoAltezza = transform.position.y - altezzaMassima;
            forzaSpinta += Vector3.down * (eccessoAltezza * intensitaFrenata * gravita * rb.mass);
        }
        else if (transform.position.y > altezzaMassima - 2f)
        {
            // Inizia a rallentare gradualmente la salita negli ultimi 2 metri
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
