using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 targetPosition;

    public void TeleportPlayer()
    {
        if (player != null)
        {
            player.transform.position = targetPosition;
        }
    }
}
