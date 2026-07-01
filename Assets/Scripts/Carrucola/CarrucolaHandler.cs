using System.Collections;
using UnityEngine;

public class CarrucolaHandler : Handler<float>
{
    [SerializeField] private CestaCarrucola cestaCarrucolaSocket;
    [SerializeField] private Step step;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;
   public override void Perform(float data)
    {
        AudioSource audioSource = target.GetComponent<AudioSource>();
        if (cestaCarrucolaSocket != null)
        {
            bool checkValue = Mathf.Abs(data - cestaCarrucolaSocket.GetEnergiaPotenziale()) <= 0.1f;
            if (checkValue)
            {
                audioSource.PlayOneShot(successSound);
                CarrucolaController carrucolaController = target.GetComponent<CarrucolaController>();
                if (carrucolaController != null)
                {
                    carrucolaController.CloseDoors();
                    carrucolaController.MovetoTop();
                    StartCoroutine(ComeDownAfterDelay(10f));
                    step.endStep();
                }
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
        yield return new WaitForSeconds(delay-0.1f);
        target.GetComponent<CarrucolaController>().OpenDoors();
    }
}
