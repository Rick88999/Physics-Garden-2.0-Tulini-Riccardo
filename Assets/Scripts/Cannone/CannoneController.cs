using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CannoneController : MonoBehaviour
{
    [SerializeField] private float launchForce = 200f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip launchSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip launchErrorSound;
    private List<GameObject> bullets;

    private void Update()
    {
        if(bullets != null && bullets.Count > 3)
        {
            GameObject bullet = bullets[0];
            if (bullet == null)
            {
                GameObject bulletToRemove = bullets[0];
                bullets.RemoveAt(0);
                Destroy(bulletToRemove);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bullets == null) bullets = new List<GameObject>();

        if (!other.CompareTag("Player") && !other.CompareTag("PlayerHand") && !other.CompareTag("Bullet") && !other.CompareTag("Cannon"))
        {
            if(other.gameObject.GetComponent<SphereCollider>() != null)
            {
                other.tag = "Bullet";
                other.gameObject.AddComponent<Bullet>();
                Bullet bullet = other.gameObject.GetComponent<Bullet>();
                bullet.SetLaunchForce(launchForce);
                bullets.Insert(0, other.gameObject); 
            }
            else
            {
                Destroy(other.gameObject);
                audioSource.PlayOneShot(launchErrorSound);
            }
        }
    }

    public void SetLaunchForceCannon(float force) => launchForce = force;
    public void Launch() 
    {
        if (bullets == null || bullets.Count == 0)
        {
            audioSource.PlayOneShot(launchErrorSound);
            return;
        }
        ;

        GameObject bullet = bullets[0];
        audioSource.PlayOneShot(launchSound);
        bullet.GetComponent<Bullet>().Shoot();
    }
    
}
