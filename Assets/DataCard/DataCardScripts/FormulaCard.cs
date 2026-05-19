using TMPro;
using UnityEngine;

public class FormulaCard : MonoBehaviour
{
    [SerializeField] private GameObject formulaCanvasData;
    [SerializeField] private string nome;
    [SerializeField] private string details;
    [SerializeField] private TextMeshProUGUI textTarghetta;


    public GameObject GetFormulaData()
    {
        return formulaCanvasData;
    }

    public void SetFormulaData(GameObject g)
    {
        formulaCanvasData = g;
    }

    public void SetTextTarghetta()
    {
        if (textTarghetta != null)
        {
            textTarghetta.text = nome;
        }
    }


    public string GetName()
    {
        return nome;
    }

    public void SetName(string name)
    {
        nome = name;
    }

    public string GetDetails()
    {
        return details;
    }

    public void SetDetails(string details)
    {
        this.details = details;
    }
}
