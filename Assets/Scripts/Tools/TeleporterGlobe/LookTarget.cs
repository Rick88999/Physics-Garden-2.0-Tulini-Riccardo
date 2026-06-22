using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(EmissionController))]
[RequireComponent(typeof(Teleport))]
public class LookTarget : MonoBehaviour
{
    public UnityEvent onActivated;
    [SerializeField] private string teleporterName;
    [SerializeField] private float resetDelay = 2f;
    private bool triggered = false;
    private EmissionController emissionController;

    private void Awake()
    {
        emissionController = GetComponent<EmissionController>();
        GameEvents.Instance.onTargetTeleport.AddListener(TargetTeleport);
        GameEvents.Instance.onDefocusTeleport.AddListener(DefocusTeleport);
    }

    public string GetTeleporterName() => teleporterName;

    public void Activate()
    {
        if (triggered) return;

        triggered = true;
        onActivated.Invoke();
        StartCoroutine(ResetTriggeredAfterDelay(resetDelay));
    }

    private IEnumerator ResetTriggeredAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        triggered = false;
    }

    private void TargetTeleport(string targetName)
    {
        if (targetName == teleporterName)
        {
            emissionController.StartEmission();
        }
    }

    private void DefocusTeleport(string targetName)
    {
        if (targetName == teleporterName)
        {
            emissionController.ResetEmission();
        }
    }
}
