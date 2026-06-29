using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayCorda : MonoBehaviour
{
    private LineRenderer ray;
    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;
    [SerializeField] private float grandSeg = 0.5f; 
    [SerializeField] private int minSeg = 3;

    private void Start() => ray = GetComponent<LineRenderer>();

    private void Update()
    {
        UpdateRope();
    }

    private void UpdateRope()
    {
        Vector3 yA = posA.position;
        Vector3 yB = posB.position;
        
        float lunghezza = Vector3.Distance(yA, yB);

        int segCount = Mathf.Max(minSeg, Mathf.RoundToInt(lunghezza / grandSeg));

        Vector3[] positions = new Vector3[segCount];
        
        for (int i = 0; i < segCount; i++)
        {
            float t = i / (float)(segCount - 1);
            positions[i] = Vector3.Lerp(yA, yB, t);
        }
        
        ray.positionCount = segCount;
        ray.SetPositions(positions);
    }
}
