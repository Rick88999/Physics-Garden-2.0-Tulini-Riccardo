using UnityEngine;

public class ActivatorGun : MonoBehaviour
{
    [SerializeField] private GravityGunController cr;
    private void Awake() => cr.Activation();

}
