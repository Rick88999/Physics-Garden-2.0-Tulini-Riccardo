using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TeleportBlockerCollider : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Transform Player;

    private Collider xrCollider;

        private void Awake() => xrCollider = GetComponent<Collider>();

    private void Update() //Getsisce il movimento del player nel mondo reale
    {
        if (Player != null && xrCollider.bounds.Contains(Player.position))
            Player.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.position = position;
    }

}