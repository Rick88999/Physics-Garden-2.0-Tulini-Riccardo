using UnityEngine;
using UnityEngine.InputSystem;

public class LaserBroomTool : MonoBehaviour
{
    private DetectOnDualGrab detectOnDualGrab;

    private void Awake() => detectOnDualGrab = GetComponentInParent<DetectOnDualGrab>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (detectOnDualGrab.IsGrabbedByBothHands())
           if (other.CompareTag("Spawned Object") || other.CompareTag("Tool")) 
                Destroy(other.gameObject);
    }
}
