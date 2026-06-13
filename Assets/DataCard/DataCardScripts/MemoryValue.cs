using UnityEngine;

public class MemoryValue : MonoBehaviour
{
    [SerializeField] private string matchNameItalian;
    [SerializeField] private string matchNameEnglish;
    [SerializeField] private float matchData;
    [Header("Tolleranza")]
    [SerializeField] private float delta;

    public bool ControlValue(string name, float value)
    {
        bool checkValue = Mathf.Abs(value - matchData) <= delta;
        if ((name == matchNameItalian || name == matchNameEnglish) && checkValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
