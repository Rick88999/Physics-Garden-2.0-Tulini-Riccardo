using UnityEngine;

public class ValueReaderHandlerController : MonoBehaviour
{
    [SerializeField] private Handler<float> handler;

    public void ExecHandler(float data) => handler.Perform(data);
    
}
