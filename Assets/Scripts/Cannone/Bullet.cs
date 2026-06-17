using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private float launchForce;
    private Vector3 launchDirection;
    private Rigidbody rb;

    public void Shoot()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
    }

    public void SetLaunchForce(float force) => launchForce = force;
    public void SetLaunchDirection(Vector3 direction) => launchDirection = direction.normalized;
    
}
