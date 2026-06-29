using UnityEngine;

public class CannonHandler : Handler<float>
{
    public override void Perform(float data) => target.GetComponent<CannoneController>().SetLaunchForceCannon(data);
}
