using UnityEngine;

public class MemoryValue : MonoBehaviour
{
    [SerializeField] private string matchName;
    [SerializeField] private float matchData;
    [Header("Tolleranza")]
    [SerializeField] private float delta;

    public bool ControlValue(string name, float value)
    {
        bool checkValue = Mathf.Abs(value - matchData) <= delta;
        if (name == matchName && checkValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
