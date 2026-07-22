using TMPro;
using UnityEngine;

public class FormulaCard : CardBase
{
    [SerializeField] private GameObject formulaCanvasData;

    public GameObject GetFormulaData()
    {
        return formulaCanvasData;
    }

    public void SetFormulaData(GameObject g)
    {
        formulaCanvasData = g;
    }

    public override void SetTextTarghetta()
    {
        if (textTarghetta != null)
        {
            textTarghetta.text = nome;
        }
    }
}
