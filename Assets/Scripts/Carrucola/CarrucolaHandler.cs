using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CarrucolaHandler : Handler<float>
{
    [Header("Riferimenti carrucola e step")]
    [SerializeField] private CestaCarrucola cestaCarrucolaSocket;
    [SerializeField] private Step step;
    [Header("Audio")]
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;
    [Header("Delay")]
    [SerializeField] private float delayMoveDown = 10f;
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
                    StartCoroutine(ComeDownAfterDelay(delayMoveDown));
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

        CarrucolaController carrucolaController = target.GetComponent<CarrucolaController>();
        carrucolaController.MovetoDown();

        yield return new WaitForSeconds(delay-0.1f);
        
        carrucolaController.OpenDoors();
    }
}
