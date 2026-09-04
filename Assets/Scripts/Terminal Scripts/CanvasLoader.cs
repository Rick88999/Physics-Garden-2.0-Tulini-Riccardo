using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class CanvasLoader : MonoBehaviour
{
    private XRSocketInteractor socket;
    private Coroutine fadeCoroutine;

    private GameObject canvasCorrente;
    private CanvasGroup cgCorrente;

    private float durataFade = 0.5f;
    private bool cardInSocket = false;
    public void ChengeCanvasAttivo(GameObject nuovoCanvas)
    {
        canvasCorrente = nuovoCanvas;
    }

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnObjectPlaced);
        socket.selectExited.AddListener(OnObjectRemoved);

        if (socket.hasSelection)
        {
            FormulaCard formulaCard = socket.GetOldestInteractableSelected().transform.GetComponent<FormulaCard>();
            if (formulaCard != null)
            {
                FormulaCanvasData itemData = formulaCard.GetFormulaData();
                if (itemData != null && itemData.canvasDaAttivare != null)
                {
                    MostraCanvas(itemData.canvasDaAttivare);
                    cardInSocket = true;
                }
            }
        }
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnObjectPlaced);
        socket.selectExited.RemoveListener(OnObjectRemoved);

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        if (canvasCorrente != null)
        {
            canvasCorrente.SetActive(false);
        }
        canvasCorrente = null;
        cgCorrente = null;
        cardInSocket = false;
    }

    private void OnObjectPlaced(SelectEnterEventArgs args)
    {
        FormulaCard formulaCard = args.interactableObject.transform.GetComponent<FormulaCard>();
        if (formulaCard == null) return;

        FormulaCanvasData itemData = formulaCard.GetFormulaData();
        if (itemData == null || itemData.canvasDaAttivare == null) return;

        cardInSocket = true;
        MostraCanvas(itemData.canvasDaAttivare);
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        if (!gameObject.activeInHierarchy || !Application.isPlaying) return;

        cardInSocket = false;
        NascondiCanvas();
    }

    private void MostraCanvas(GameObject nuovoCanvas)
    {
        if (canvasCorrente != null && canvasCorrente != nuovoCanvas)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

            if (cgCorrente != null) cgCorrente.alpha = 0f;
            canvasCorrente.SetActive(false);
        }
        else if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        canvasCorrente = nuovoCanvas;
        cgCorrente = canvasCorrente.GetComponent<CanvasGroup>();
        if (cgCorrente == null)
        {
            cgCorrente = canvasCorrente.AddComponent<CanvasGroup>();
        }

        canvasCorrente.SetActive(true);
        fadeCoroutine = StartCoroutine(EseguiFade(cgCorrente, canvasCorrente, 1f, true));
    }

    private void NascondiCanvas()
    {
        if (canvasCorrente == null) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(EseguiFade(cgCorrente, canvasCorrente, 0f, false));
    }

    private IEnumerator EseguiFade(CanvasGroup cg, GameObject canvas, float targetAlpha, bool attivazione)
    {
        float startAlpha = cg.alpha;
        float time = 0f;

        while (time < durataFade)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / durataFade);
            yield return null;
        }

        cg.alpha = targetAlpha;

        if (!attivazione)
        {
            canvas.SetActive(false);

            if (canvasCorrente == canvas)
            {
                canvasCorrente = null;
                cgCorrente = null;
            }
        }

        fadeCoroutine = null;
    }

    public bool IsCardInSocket() => cardInSocket;
}
