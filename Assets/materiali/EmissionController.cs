using System.Collections;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [ColorUsage(true, true)] // abilita il color picker HDR nell'Inspector
    [SerializeField] private Color baseEmissionColor = Color.white;
    [SerializeField] private float targetIntensity = 5f; // moltiplicatore finale
    [SerializeField] private float duration = 3f;

    private Material mat;
    private float currentIntensity = 0f;
    private static readonly int EmissionID = Shader.PropertyToID("_EmissionColor");

    void Awake()
    {
        targetRenderer = GetComponent<Renderer>();
        mat = targetRenderer.material; // istanza, non tocca l'asset condiviso
        mat.EnableKeyword("_EMISSION"); // assicura che l'emissione sia attiva
    }

    public void StartEmission()
    {
        StopAllCoroutines();
        StartCoroutine(EmissionRoutine());
    }

    public void ResetEmission()
    {
        StopAllCoroutines();
        StartCoroutine(EmissionFadeOutRoutine());
    }

    private IEnumerator EmissionRoutine()
    {
        float startIntensity = currentIntensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            mat.SetColor(EmissionID, baseEmissionColor * currentIntensity);
            yield return null;
        }

        currentIntensity = targetIntensity;
        mat.SetColor(EmissionID, baseEmissionColor * currentIntensity);
    }

    private IEnumerator EmissionFadeOutRoutine()
    {
        float startIntensity = currentIntensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentIntensity = Mathf.Lerp(startIntensity, 0f, elapsed / duration);
            mat.SetColor(EmissionID, baseEmissionColor * currentIntensity);
            yield return null;
        }

        currentIntensity = 0f;
        mat.SetColor(EmissionID, Color.black);
    }
}
