using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ModuloPuroRotolamento : MonoBehaviour
{
    private float radius;
    private Rigidbody corpo;

    [Header("Parametri Fisici")]
    public bool forzareInerziaTeorica = true;

    void Start()
    {
        corpo = GetComponent<Rigidbody>();

        // 1. Pulizia parametri fisici per evitare attriti dell'aria
        corpo.linearDamping = 0f;
        corpo.angularDamping = 0f;

        // 2. Calcolo raggio preciso
        if (GetComponent<SphereCollider>())
            radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
        else if (GetComponent<CapsuleCollider>())
            radius = GetComponent<CapsuleCollider>().radius * transform.localScale.x;

        // 4. Configurazione Tensore d'Inerzia
        if (forzareInerziaTeorica)
        {
            ConfiguraInerzia();
        }
    }

    void ConfiguraInerzia()
    {
        float m = corpo.mass;
        float r2 = radius * radius;
        float inerziaVal = 0f;

        // Applichiamo i coefficienti k corretti
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

    void FixedUpdate()
    {
        // Non forziamo più la velocità angolare.
        // Lasciamo che il motore fisico calcoli la velocità in base all'inerzia.

        float v = corpo.linearVelocity.magnitude;
        float omega = corpo.angularVelocity.magnitude;

        // Debug per verificare se il rotolamento è coerente (v = omega * r)
        if (v > 0.1f)
        {
            float errore = Mathf.Abs(v - (omega * radius));
            // Se l'errore è alto, significa che l'oggetto sta slittando 
            // e serve più attrito nel PhysicMaterial.
            Debug.Log($"{gameObject.name} | Vel: {v:F2} | Errore Rotolamento: {errore:F3}");
        }
    }
}
