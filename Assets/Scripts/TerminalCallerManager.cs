using UnityEngine;

public class TerminalCallerManager : MonoBehaviour
{
    [SerializeField] private GameObject lastTerminalCaller;

    public void SetLastTerminalCaller(GameObject terminalCaller)
    {
        if (lastTerminalCaller != null && lastTerminalCaller != terminalCaller)
        {
            VRInterruptor interruptor = lastTerminalCaller.GetComponent<VRInterruptor>();
            if (interruptor != null) interruptor.ResetActualEvent();
            lastTerminalCaller = terminalCaller;
        }
        else if (lastTerminalCaller == null)
        {
            lastTerminalCaller = terminalCaller;
        }
    }
}
