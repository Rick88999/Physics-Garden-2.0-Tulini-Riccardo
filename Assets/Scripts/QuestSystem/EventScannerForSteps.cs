using Unity.VisualScripting;
using UnityEngine;

public class EventScannerForSteps : MonoBehaviour
{
    [SerializeField] private Event tipoEvento;
    [SerializeField] private Step stepPointer;
    [SerializeField] private string toolNameToCheck;

    private void OnEnable()
    {
        if (tipoEvento != Event.SpawnTool) return;
        if (GameEvents.Instance?.onToolSpawned == null) return;

        if (tipoEvento == Event.SpawnTool)
        GameEvents.Instance.onToolSpawned.AddListener(NextStep);
    }

    private void OnDisable()
    {
        if(tipoEvento == Event.SpawnTool)
        GameEvents.Instance.onToolSpawned.RemoveListener(NextStep);
    }

    private void NextStep(string toolName) { if(toolName == toolNameToCheck || toolName.Contains(toolNameToCheck)) if(stepPointer != null) stepPointer.endStep(); }  
}
