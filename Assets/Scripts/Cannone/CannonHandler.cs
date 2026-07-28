using UnityEngine;

public class CannonHandler : Handler<float>
{
    public override void Perform(float data)
    {
        CannoneController cannoneController = target.GetComponent<CannoneController>();
        cannoneController.SetLaunchForceCannon(data);
    }
     
}
