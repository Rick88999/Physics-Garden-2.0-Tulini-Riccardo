using UnityEngine;

public class CanvasRotation : MonoBehaviour
{
    [SerializeField] public Transform player;
    void Update()
    {
        if (player == null) return;
        else
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            transform.Rotate(0, 180, 0);
        }
            
    }
}
