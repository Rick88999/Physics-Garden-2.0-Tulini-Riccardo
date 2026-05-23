using UnityEngine;

public class MemoryValue : MonoBehaviour
{
    [SerializeField] private string matchName;
    [SerializeField] private float matchData;

    public bool ControlValue(string name, float value)
    {
        if (name == matchName && value == matchData)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
