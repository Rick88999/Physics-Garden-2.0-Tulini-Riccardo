using UnityEngine;
using UnityEngine.InputSystem;

public class LaserBroomTool : VRToolBothHands
{
    protected override void Awake() => base.Awake();
    
    private void OnTriggerEnter(Collider other)
    {
        if (detectOnDualGrab.IsGrabbedByBothHands())
           if (other.CompareTag("Spawned Object") || other.CompareTag("Tool")) 
                Destroy(other.gameObject);
    }
}
