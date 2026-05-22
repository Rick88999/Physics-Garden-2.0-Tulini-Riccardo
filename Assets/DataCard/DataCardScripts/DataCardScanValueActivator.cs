using UnityEngine;

public class DataCardScanValueActivator : MonoBehaviour
{
    [SerializeField] private float matchData;
    [SerializeField] private string matchName;
    [SerializeField] private Step Step;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSoundError;
    [SerializeField] private AudioClip clickSoundSuccess;
    public void ControlValue(string name, float value)
    {
        if (name == matchName && value == matchData)
        {
            if (audioSource != null && clickSoundSuccess != null)
            {
                audioSource.PlayOneShot(clickSoundSuccess);
            }
            Step.endStep();
        }
        else
        {
            if (audioSource != null && clickSoundError != null)
            {
                audioSource.PlayOneShot(clickSoundError);
            }
            Debug.LogWarning("Valore o nome non corrispondente. Step non attivato.");
        }

    }
}
