using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    [Header("Scroll UV")]
    public float scrollSpeedX = 0.05f;
    public float scrollSpeedY = 0.02f;

    [Header("Ondulazione (opzionale)")]
    public float waveHeight = 0.05f;
    public float waveSpeed = 1.5f;

    private Material mat;
    private Vector3 startPosition;

    void Start()
    {
        // In URP si usa GetComponent<Renderer>().material
        mat = GetComponent<Renderer>().material;
        startPosition = transform.position;
    }

    void Update()
    {
        AnimateUVScroll();
        AnimateWave();
    }

    void AnimateUVScroll()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        // In URP la Base Map si chiama "_BaseMap"
        mat.SetTextureOffset("_BaseMap", new Vector2(offsetX, offsetY));

        // Se hai anche la Normal Map, animala a velocità diversa
        mat.SetTextureOffset("_BumpMap", new Vector2(offsetX * 0.5f, offsetY * 0.5f));
    }

    void AnimateWave()
    {
        // Muove leggermente il cubo su e giù per simulare il moto ondoso
        float newY = startPosition.y + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }
}
