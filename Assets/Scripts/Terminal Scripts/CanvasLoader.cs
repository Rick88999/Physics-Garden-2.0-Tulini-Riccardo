using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class CanvasLoader : MonoBehaviour
{

    private XRSocketInteractor socket;
    private CanvasGroup canvasGroupAttuale;
    private Coroutine fadeCoroutine;
    private GameObject canvasAttivo;
    private float durataFade = 0.5f;

    public void ChengeCanvasAttivo(GameObject nuovoCanvas)
    {
        canvasAttivo = nuovoCanvas;
    }

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnObjectPlaced);
        socket.selectExited.AddListener(OnObjectRemoved);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnObjectPlaced);
        socket.selectExited.RemoveListener(OnObjectRemoved);
    }

    private void OnObjectPlaced(SelectEnterEventArgs args)
    { 
        FormulaCard formulaCard = args.interactableObject.transform.GetComponent<FormulaCard>();
        FormulaCanvasData itemData = formulaCard.GetFormulaData();

        if (itemData != null && itemData.canvasDaAttivare != null)
        {
            canvasAttivo = itemData.canvasDaAttivare;
            //canvasAttivo.SetActive(true);
        }

        canvasGroupAttuale = canvasAttivo.GetComponent<CanvasGroup>();
        if (canvasGroupAttuale == null)
        {
            canvasGroupAttuale = canvasAttivo.AddComponent<CanvasGroup>();
        }

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(EseguiFade(1f, true));
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        /*if (canvasAttivo != null)
        {
            canvasAttivo.SetActive(false);
            canvasAttivo = null; 
        }*/

        if (canvasAttivo != null && canvasGroupAttuale != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(EseguiFade(0f, false));
        }
    }

    private IEnumerator EseguiFade(float targetAlpha, bool attivazione)
    {
        if (attivazione)
        {
            canvasAttivo.SetActive(true);
            canvasGroupAttuale.alpha = 0f;
        }

        float startAlpha = canvasGroupAttuale.alpha;
        float time = 0f;

        while (time < durataFade)
        {
            time += Time.deltaTime;
            canvasGroupAttuale.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / durataFade);
            yield return null;
        }

        canvasGroupAttuale.alpha = targetAlpha;

        if (!attivazione)
        {
            canvasAttivo.SetActive(false);
            canvasAttivo = null;
            canvasGroupAttuale = null;
        }
    }
}
