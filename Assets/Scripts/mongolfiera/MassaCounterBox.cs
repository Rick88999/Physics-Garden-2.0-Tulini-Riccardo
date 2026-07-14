using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MassaCounterBox : MonoBehaviour
{
    [SerializeField] private GameObject led;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private float errorBound = 15f;
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip onSound;
    
    private BoxCollider boxCollider;
    private float massaTotale = 0f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("MassaCounterBox: Non è stato trovato un Collider sul GameObject.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && !(rb.CompareTag("Tool") || rb.CompareTag("Player") || rb.CompareTag("PlayerHand") || rb.CompareTag("FormulaCard") || rb.CompareTag("DataCard")))
        {
            massaTotale += rb.mass;
            other.tag = "No Destroy";
            Debug.Log("Massa totale nella box: " + massaTotale);
        }

        if(rb != null && (rb.CompareTag("Tool") || rb.CompareTag("FormulaCard") || rb.CompareTag("DataCard")))
            Destroy(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && !(rb.CompareTag("Tool") || rb.CompareTag("Player") || rb.CompareTag("PlayerHand") || rb.CompareTag("FormulaCard") || rb.CompareTag("DataCard")))
        {
            massaTotale -= rb.mass;
            other.tag = "Spawned Object";
            Debug.Log("Massa totale nella box: " + massaTotale);
        }
    }

    public bool TooMuchMass()
    {
        if (massaTotale >= errorBound)
        {
            led.GetComponent<Renderer>().material = red;
            Image img = panel.GetComponent<Image>();
            img.color = Color.red;
            txt.text = "No mass > 15kg";
            StartCoroutine(SetUp());

            audioSource.PlayOneShot(onSound);
            return true;
        }
        else if (massaTotale < errorBound)
            led.GetComponent<Renderer>().material = green;

        return false;
        
        
    }

    private IEnumerator SetUp()
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(5f);
        led.GetComponent<Renderer>().material = green;
        panel.SetActive(false);
    }

    public float GetErrorBound()
    {
        if(errorBound == 0) return 1f;
        else return errorBound;
    }
        

    public float GetMassaTotale()
    {
        if (massaTotale == 0) return 1;
        else return massaTotale;
        
    }
}

