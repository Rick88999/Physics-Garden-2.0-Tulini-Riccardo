using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CannoneController : MonoBehaviour
{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip launchSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip launchErrorSound;

    [SerializeField] private bool tryShoot;

    private GameObject currentBullet;
    private List<GameObject> bulletsAlreadyLaunched = new List<GameObject>();

    private void Update()
    {
        if(bulletsAlreadyLaunched != null && bulletsAlreadyLaunched.Count > 3)
        {
            GameObject bulletToRemove = bulletsAlreadyLaunched[0];
            bulletsAlreadyLaunched.RemoveAt(0);
            Destroy(bulletToRemove);
        }

        if(tryShoot && currentBullet != null)
        {
            Launch();
            tryShoot = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player") && !other.CompareTag("PlayerHand") && !other.CompareTag("Bullet") && !other.CompareTag("Cannon"))
        {
            if(other.gameObject.GetComponent<SphereCollider>() != null)
            {
                if (currentBullet != null)
                {
                    Destroy(currentBullet);
                }

                other.tag = "Bullet";
                other.gameObject.AddComponent<Bullet>();
                Bullet bullet = other.gameObject.GetComponent<Bullet>();
                currentBullet = other.gameObject;
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
        if (currentBullet == null)
        {
            audioSource.PlayOneShot(launchErrorSound);
            return;
        };
        
        audioSource.PlayOneShot(launchSound);

        Collider bulletCollider = currentBullet.GetComponent<Collider>();
        Collider cannonCollider = GetComponent<Collider>();
        if (bulletCollider != null && cannonCollider != null)
            Physics.IgnoreCollision(bulletCollider, cannonCollider, true);

        Bullet bulletComponent = currentBullet.GetComponent<Bullet>();

        bulletComponent.SetLaunchDirection(transform.forward);
        bulletComponent.SetLaunchForce(launchForce);

        bulletComponent.Shoot();
        bulletsAlreadyLaunched.Add(currentBullet);

        currentBullet = null;
    }
    
}
