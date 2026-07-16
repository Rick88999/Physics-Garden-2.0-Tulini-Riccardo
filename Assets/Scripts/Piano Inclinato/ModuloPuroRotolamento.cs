using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ModuloPuroRotolamento : MonoBehaviour
{
    private float radius;
    private Rigidbody corpo;

    [Header("Parametri Fisici")]
    [SerializeField] private bool forzareInerziaTeorica = true;

    private void Awake()
    {
        corpo = GetComponent<Rigidbody>();

        // Pulizia parametri fisici per evitare attriti dell'aria
        corpo.linearDamping = 0f;
        corpo.angularDamping = 0f;

        // Calcolo raggio preciso
        if (GetComponent<SphereCollider>())
            radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
        else if (GetComponent<CapsuleCollider>())
            radius = GetComponent<CapsuleCollider>().radius * transform.localScale.x;

        // Configurazione Tensore d'Inerzia
        if (forzareInerziaTeorica)
        {
            ConfiguraInerzia();
        }
    }

    private void ConfiguraInerzia()
    {
        float m = corpo.mass;
        float r2 = radius * radius;
        float inerziaVal = 0f;

        // Applico i coefficienti k corretti
        if (corpo.name.ToLower().Contains("sfera"))
        {
            inerziaVal = 0.4f * m * r2; // k = 2/5
        }
        else if (corpo.name.ToLower().Contains("cilindro"))
        {
            inerziaVal = 0.5f * m * r2; // k = 1/2
        }

        if (inerziaVal > 0)
        {
            corpo.inertiaTensor = new Vector3(inerziaVal, inerziaVal, inerziaVal);
            // Reset della rotazione del tensore per allinearlo agli assi locali
            corpo.inertiaTensorRotation = Quaternion.identity;
        }
    }

    private void FixedUpdate()
    {
        // Imposto il motore fisico per calcolre l'inerzia corretta e il rotolamento puro

        float v = corpo.linearVelocity.magnitude;
        float omega = corpo.angularVelocity.magnitude;

        if (v > 0.1f)
        {
            float errore = Mathf.Abs(v - (omega * radius));
            Debug.Log($"{gameObject.name} | Vel: {v:F2} | Errore Rotolamento: {errore:F3}");
        }
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onInclinedPlaneObjectDestroyed?.Invoke(GetInstanceID());
    }
}
