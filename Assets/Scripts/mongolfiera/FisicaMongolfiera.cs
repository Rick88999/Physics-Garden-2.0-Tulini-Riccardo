using UnityEngine;
public class FisicaMongolfiera : MonoBehaviour
{
    private Rigidbody rb;
    public bool activator;
    [SerializeField] private MassaCounterBox massaCounterBox;
    private float massaTotale = 1f;
    private readonly float terra = 0f;
    private float gravita = Mathf.Abs(Physics.gravity.y);
    [Header("Parametri Fisici")]
    [SerializeField] private float volumeOggetto;
    [SerializeField] private float densitaFluido;
    [SerializeField] private float altezzaMassima;
    [SerializeField] private int altezzaAttesa;
    [Header("Avvio Graduale")]
    [SerializeField] private float durataAvvio = 3f;
    private float tempoAvvio = 0f;
    [Header("Tappo Invisibile")]
    [SerializeField] private GameObject Tappo;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.linearDamping = 8f;
        altezzaAttesa = (int)altezzaMassima;
        massaTotale = massaCounterBox != null ? massaCounterBox.GetMassaTotale() : 1f;
    }

    void FixedUpdate()
    {
        if (activator)
        {
            tempoAvvio += Time.fixedDeltaTime;
            float fattoreAvvio = Mathf.Clamp01(tempoAvvio / durataAvvio);

            float distanza = altezzaAttesa - transform.position.y;

            float accelerazioneeMinima = Mathf.Clamp(Mathf.Abs(distanza) * 15f, 3f, 50f);
            float forzaVersTarget = Mathf.Sign(distanza) * accelerazioneeMinima * massaTotale * fattoreAvvio;

            float compensazioneGravita = massaTotale * gravita;

            Vector3 forzaTotale = Vector3.up * (forzaVersTarget + compensazioneGravita);
            rb.AddForce(forzaTotale, ForceMode.Force);

            float vicinanzaTarget = 1f - Mathf.Clamp01(Mathf.Abs(distanza) / 5f);
            rb.linearDamping = Mathf.Lerp(2f, 15f, vicinanzaTarget);
        }
    }

    public void AttivaDisattivaSpinta()
    {
        if (massaCounterBox == null)
        {
            Debug.LogError("FisicaMongolfiera: MassaCounterBox non assegnato.");
            return;
        }

        if(massaCounterBox.TooMuchMass()) return;
        
        if (activator && Mathf.Abs(transform.position.y - altezzaAttesa) < 1f)
        {
            activator = false;
            Tappo.SetActive(false);
            tempoAvvio = 0f;
        }
        else if (Mathf.Abs(transform.position.y - terra) < 0.1f)
        {
            massaTotale = massaCounterBox != null ? massaCounterBox.GetMassaTotale() : 1f;
            if (Mathf.Abs((volumeOggetto * densitaFluido) - massaTotale) <= 0.01f)
            {
                activator = false;
                Tappo.SetActive(false);
                return;
            }
            float spintaArchimede = volumeOggetto * densitaFluido * gravita;
            if(massaTotale == 0) altezzaAttesa = (int)altezzaMassima;
            else altezzaAttesa = (int)altezzaMassima - (int)(altezzaMassima * ((massaTotale * gravita) / spintaArchimede));

            if(altezzaAttesa>0 && altezzaAttesa<1) altezzaAttesa = 1;

            if ((massaTotale > volumeOggetto*densitaFluido && massaTotale < massaCounterBox.GetErrorBound()) || massaTotale > massaCounterBox.GetErrorBound())
            {
                tempoAvvio = 0f;
                Tappo.SetActive(false);
                activator = false;
            }
            else if (massaTotale < volumeOggetto * densitaFluido)
            {
                tempoAvvio = 0f;
                Tappo.SetActive(true);
                activator = true;
            }
        }
    }
}
