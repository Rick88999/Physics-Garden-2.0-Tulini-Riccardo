using TMPro;
using UnityEngine;

public class FormulaCard : CardBase
{
    [SerializeField] private GameObject formulaCanvasData;

    public FormulaCanvasData GetFormulaData()
    {
        FormulaCanvasData canvasData = this.formulaCanvasData.GetComponent<FormulaCanvasData>();
        return canvasData;
    }

    public void SetFormulaData(GameObject g) => formulaCanvasData = g;

    public override void SetTextTarghetta()
    {
        if (textTarghetta != null)
        {
            textTarghetta.text = nome;
        }
    }
}
