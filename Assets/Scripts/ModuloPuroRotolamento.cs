using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ModuloPuroRotolamento : MonoBehaviour
{
    private float radius;
    private Rigidbody corpo;
    private string nomeCorpo;

    void Start()
    {
        corpo = GetComponent<Rigidbody>();
        corpo.angularDamping = 0f;
        nomeCorpo = gameObject.name;
        if (nomeCorpo.Contains("Sfera")) radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
        else if (nomeCorpo.Contains("Cilindro")) radius = GetComponent<CapsuleCollider>().radius * transform.localScale.x;
    }

    void FixedUpdate()
    {
        Vector3 velocita = corpo.linearVelocity;
        if (velocita.magnitude < 0.001f) return;

        Vector3 normale = Vector3.up;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, radius + 0.05f))
            normale = hit.normal;

        Vector3 asseRotolamento = Vector3.Cross(velocita, normale).normalized;
        corpo.angularVelocity = asseRotolamento * (velocita.magnitude / radius);
        Debug.Log($"{nomeCorpo} velocità: {velocita.magnitude}, omega: {corpo.angularVelocity.magnitude}, radius: {radius}");

#if UNITY_EDITOR
        float v = velocita.magnitude;
        float omega = corpo.angularVelocity.magnitude;
        float diff = Mathf.Abs(v - omega * radius);
        if (diff > 0.01f)
            Debug.LogWarning($"{nomeCorpo} errore rotolamento: {diff} => v: {v}, omega: {omega}, radius: {radius}");
#endif
    }
}
