using UnityEngine;
public class FisicaMongolfiera : MonoBehaviour
{
    private Rigidbody rb;
    public bool activator;
    [SerializeField] private MassaCounterBox massaCounterBox;
    private float massaTotale = 1f;
    private readonly float terra = -0.5f;
    private float gravita = Mathf.Abs(Physics.gravity.y);
    [Header("Parametri Fisici")]
    [SerializeField] private float volumeOggetto;
    [SerializeField] private float densitaFluido;
    [SerializeField] private float altezzaMassima;
    [SerializeField] private float altezzaAttesa;
    [Header("Controllo Altezza")]
    [SerializeField] private float intensitaFrenata = 2f;
    [Header("Avvio Graduale")]
    [SerializeField] private float durataAvvio = 3f;
    private float tempoAvvio = 0f;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.linearDamping = 8f;
        altezzaAttesa = altezzaMassima;
        massaTotale = massaCounterBox != null ? massaCounterBox.GetMassaTotale() : 1f;
    }

    void FixedUpdate()
    {
        if (activator)
        {
            tempoAvvio += Time.fixedDeltaTime;
            float fattoreAvvio = Mathf.Clamp01(tempoAvvio / durataAvvio);

            float spintaArchimede = volumeOggetto * densitaFluido * gravita;
            Vector3 forzaSpinta = Vector3.up * spintaArchimede * fattoreAvvio;

            if (transform.position.y > altezzaAttesa)
            {
                float eccessoAltezza = transform.position.y - altezzaAttesa;
                forzaSpinta += Vector3.down * (eccessoAltezza * eccessoAltezza * intensitaFrenata * gravita * massaTotale);
                rb.linearDamping = Mathf.Lerp(8f, 1.5f, fattoreAvvio);
            }
            else if (transform.position.y > altezzaAttesa - 4f)
            {
                float fattoreRallentamento = (transform.position.y - (altezzaAttesa - 2f)) / 2f;
                rb.linearDamping = Mathf.Lerp(1.5f, 12f, fattoreRallentamento);
            }
            else
            {
                rb.linearDamping = Mathf.Lerp(6f, 1.5f, fattoreAvvio);
            }

            rb.AddForce(forzaSpinta, ForceMode.Force);
        }
    }

    public void AttivaDisattivaSpinta()
    {
        if (Mathf.Abs(transform.position.y - altezzaAttesa) < 3f)
        {
            activator = false;
            tempoAvvio = 0f;
        }
        else if (Mathf.Abs(transform.position.y - terra) < 0.1f)
        {
            massaTotale = massaCounterBox != null ? massaCounterBox.GetMassaTotale() : 1f;
            if (Mathf.Abs((volumeOggetto * densitaFluido) - massaTotale) <= 0.01f)
            {
                activator = false;
                return;
            }
            float spintaArchimede = volumeOggetto * densitaFluido * gravita;
            altezzaAttesa = altezzaMassima - (altezzaMassima * ((massaTotale * gravita) / spintaArchimede));
            tempoAvvio = 0f;
            activator = true;
        }
    }
}
