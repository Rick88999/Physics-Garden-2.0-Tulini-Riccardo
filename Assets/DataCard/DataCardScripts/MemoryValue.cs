using UnityEngine;

public class MemoryValue : MonoBehaviour
{
    [SerializeField] private string matchNameItalian;
    [SerializeField] private string matchNameEnglish;
    [SerializeField] private float matchData;
    [SerializeField] private string unitaDiMisura;
    [Header("Tolleranza")]
    [SerializeField] private float delta;


    public bool ControlValue(string name, string unita, float value)
    {
        bool checkValue = Mathf.Abs(value - matchData) <= delta;
        bool checkUnita = unita == unitaDiMisura;
        if ((name == matchNameItalian || name == matchNameEnglish) && checkValue && checkUnita)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
