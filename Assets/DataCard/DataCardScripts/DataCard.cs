using TMPro;
using UnityEngine;

public class DataCard : CardBase
{
    [SerializeField] private float valore;
    [SerializeField] private string unitaMisura;

    public float GetValue() => valore;

    public void SetValue(float value) => valore = value;

    public override void SetTextTarghetta()
    {
        if (textTarghetta != null)
        {
            textTarghetta.text = nome+":\n "+valore.ToString()+" "+unitaMisura;
        }
    }

    public string GetUnitaMisura() => unitaMisura;
    

    public void SetUnitaMisura(string unit) => unitaMisura = unit;
    

    
}
