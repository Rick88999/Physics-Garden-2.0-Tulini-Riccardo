using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;

public class VRHeadsetDetector : MonoBehaviour
{
    [SerializeField] private VRHeadsetType headsetType;
    
    public VRHeadsetType DetectVRHeadset()
    {
        string runtimeName = OpenXRRuntime.name;
    
        switch (runtimeName)
        {
            case string name when name.Contains("Meta") || name.Contains("Oculus"):
                Debug.Log("VR Headset: Meta Quest");
                headsetType = VRHeadsetType.MetaQuest;
                break;
            case string name when name.Contains("Vive") || name.Contains("HTC") || name.Contains("Steam"):
                Debug.Log("VR Headset: HTC Vive CE");
                headsetType = VRHeadsetType.HTCViveCE;
                break;
            default:
                Debug.Log("VR Headset: Unknown prob standAlone");
                headsetType = VRHeadsetType.MetaQuest;
                break;
        }

        return headsetType;
        
    }

}
