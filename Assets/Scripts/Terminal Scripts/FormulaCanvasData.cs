using UnityEngine;

public class FormulaCanvasData : MonoBehaviour
{
    public GameObject canvasDaAttivare;

    void Awake()
    {
        if (canvasDaAttivare != null)
        {
            canvasDaAttivare.SetActive(false);
        }
    }
}
