using UnityEngine;

public class FormulaCanvasData : MonoBehaviour
{
    public GameObject canvasDaAttivare;

    private void Awake()
    {
        if (canvasDaAttivare != null)
        {
            canvasDaAttivare.SetActive(false);
        }
    }

    public void SetCanvas(GameObject NewCanvas)
    {
        canvasDaAttivare = NewCanvas;
    }
}
