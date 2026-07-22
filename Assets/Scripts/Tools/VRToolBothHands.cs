using UnityEngine;

public abstract class VRToolBothHands : MonoBehaviour
{
    protected DetectOnDualGrab detectOnDualGrab;

    protected virtual void Awake() => detectOnDualGrab = GetComponentInParent<DetectOnDualGrab>();
}
