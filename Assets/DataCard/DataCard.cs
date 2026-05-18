using TMPro;
using UnityEngine;

public class DataCard : MonoBehaviour
{
    [SerializeField]private float valore;
    [SerializeField]private string nome;
    [SerializeField]private string unit‡Misura;
    [SerializeField] private string details;
    [SerializeField] private TextMeshProUGUI textTarghetta;


    public float GetValue()
    {
        return valore;
    }

    public void SetValue(float value)
    {
        valore = value;
    }

    public void SetTextTarghetta()
    {
        if (textTarghetta != null)
        {
            textTarghetta.text = nome+":\n "+valore.ToString()+" "+unit‡Misura;
        }
    }

    public string GetUnit‡Misura()
    {
        return unit‡Misura;
    }

    public void SetUnitaMisura(string unit)
    {
        unit‡Misura = unit;
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
