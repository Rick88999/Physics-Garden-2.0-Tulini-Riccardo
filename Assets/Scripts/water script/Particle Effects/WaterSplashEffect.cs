using UnityEngine;

public class WaterSplashEffect : MonoBehaviour
{
    [Header("Effetto Splash")]
    public GameObject splashPrefab;      // trascina WaterSplash qui
    public float minVelocityForSplash = 1.5f; // velocità minima per triggherare

    [Header("Cerchi sull'acqua")]
    public GameObject ripplePrefab;      // cerchio che si espande
    public int maxRipples = 3;           // numero di cerchi concentrici

    void OnTriggerEnter(Collider other)
    {
        // Ignora il player e oggetti statici
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        float impactSpeed = rb.linearVelocity.magnitude;
        if (impactSpeed < minVelocityForSplash) return;

        // Punto di impatto sulla superficie dell'acqua
        Vector3 impactPoint = new Vector3(
            other.transform.position.x,
            transform.position.y,   // Y della superficie acqua
            other.transform.position.z
        );
        Debug.Log($"Impact at {impactPoint} with speed {impactSpeed}");

        // Spawna lo splash
        SpawnSplash(impactPoint, impactSpeed, other.bounds.size);

        // Spawna i cerchi concentrici
        SpawnRipples(impactPoint, impactSpeed);
    }

    void SpawnSplash(Vector3 point, float speed, Vector3 objSize)
    {
        if (splashPrefab == null) return;

        GameObject splash = Instantiate(splashPrefab, point, Quaternion.identity);
        ParticleSystem ps = splash.GetComponent<ParticleSystem>();

        // Scala lo splash in base alla velocità e dimensione oggetto
        var main = ps.main;
        float scale = Mathf.Clamp(speed * 0.15f, 0.5f, 2.5f);
        main.startSpeedMultiplier = scale * 2f;
        main.startSizeMultiplier = scale;

        ps.Play();
        Debug.Log($"Spawned splash at {point} with scale {scale}");
        Destroy(splash, 2f); // distrugge dopo 2 secondi
    }

    void SpawnRipples(Vector3 point, float speed)
    {
        if (ripplePrefab == null) return;

        for (int i = 0; i < maxRipples; i++)
        {
            float delay = i * 0.15f; // ogni cerchio parte leggermente dopo
            Debug.Log($"Scheduling ripple {i} at {point} with delay {delay}");
            StartCoroutine(SpawnRippleDelayed(point, delay, i));
        }
    }

    System.Collections.IEnumerator SpawnRippleDelayed(Vector3 point, float delay, int index)
    {
        yield return new WaitForSeconds(delay);

        GameObject ripple = Instantiate(ripplePrefab, point, Quaternion.identity);
        StartCoroutine(ExpandRipple(ripple, index));
    }

    System.Collections.IEnumerator ExpandRipple(GameObject ripple, int index)
    {
        float duration = 1.2f;
        float elapsed = 0f;
        float maxScale = 2f + index * 0.5f;

        Renderer rend = ripple.GetComponent<Renderer>();

        // Crea una copia privata del materiale per non modificare il prefab
        Material mat = rend.material;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Espansione solo su X e Z, NON su Y (rimane piatto)
            float scale = Mathf.Lerp(0.1f, maxScale, t);
            ripple.transform.localScale = new Vector3(scale, 0.01f, scale);

            // Fade alpha
            Color c = mat.color;
            c.a = Mathf.Lerp(0.6f, 0f, t);
            mat.color = c;

            yield return null;
        }

        Destroy(ripple);
    }
}