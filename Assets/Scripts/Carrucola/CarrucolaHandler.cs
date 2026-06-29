using System.Collections;
using UnityEngine;

public class CarrucolaHandler : Handler<float>
{
    [SerializeField] private CestaCarrucola cestaCarrucola;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;
   public override void Perform(float data)
    {
        AudioSource audioSource = target.GetComponent<AudioSource>();
        if (cestaCarrucola != null)
        {
            bool checkValue = Mathf.Abs(data - cestaCarrucola.GetEnergiaPotenziale()) <= 0.1f;
            if (checkValue)
            {
                audioSource.PlayOneShot(successSound);
                target.GetComponent<CarrucolaController>().MovetoTop();
                StartCoroutine(ComeDownAfterDelay(10f));
            }
            else
            {
                audioSource.PlayOneShot(errorSound);
            }
        }
    }

    private IEnumerator ComeDownAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        target.GetComponent<CarrucolaController>().MovetoDown();
    }
}
