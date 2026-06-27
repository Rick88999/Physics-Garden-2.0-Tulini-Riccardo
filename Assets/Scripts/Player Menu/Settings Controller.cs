using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Settings References")]
    [SerializeField] private bool isStartMenu;
    [Header("Light Settings")]
    [SerializeField] private Light mainLight;
    [SerializeField] private List<Light> spotLights;
    [SerializeField] private Slider intensityLight;
    [Header("Audio Settings")]
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private Slider volumeSlider;
    [Header("Audio NPC")]
    [SerializeField] private List<AudioSource> npcAudioSources;
    [Header("Audio Objects")]
    [SerializeField] private float objectsVolumeMultiplier = 2f;
    [SerializeField] private List<AudioSource> objectsAudioSources;
  
    

    private Volume volume;
    private Bloom bloom;

    private void Start()
    {
        Volume[] volumes = FindObjectsByType<Volume>(FindObjectsSortMode.None);

        if (volumes.Length > 0)
        {
            volume = volumes[0];

            if (volume.profile != null)
            {
                volume.profile.TryGet<Bloom>(out bloom);
            }
        }
        intensityLight.onValueChanged.AddListener(SetLightIntensity);
        volumeSlider.onValueChanged.AddListener(SetAudioVolume);
        if (isStartMenu)
        {
            if (spotLights != null && spotLights.Count > 0)
                intensityLight.value = spotLights[0].intensity * 0.001666f;
        }
        
        volumeSlider.value = mainAudioSource.volume;
    }

    public void SetLightIntensity(float value)
    {
        if (!isStartMenu)mainLight.intensity = value*3;
        if (spotLights != null)
        {
            foreach (Light spotLight in spotLights)
            {
                spotLight.intensity = value * 500;
            }
        }
        if (bloom != null) bloom.intensity.value = value * 0.25f;

    }

    public void SetAudioVolume(float value)
    {
        mainAudioSource.volume = value;
        if(npcAudioSources != null)
        {
            foreach(AudioSource npcAudio in npcAudioSources)
            {
                npcAudio.volume = Mathf.Min(value, 0.7f);
            }
        }
        if(objectsAudioSources != null)
        {
            foreach(AudioSource objectAudio in objectsAudioSources)
            {
                objectAudio.volume = Mathf.Clamp01(value * objectsVolumeMultiplier);
            }
        }
    }

    public void InsertNewToolInList(AudioSource newTool)
    {
        if (!objectsAudioSources.Contains(newTool))
        {
            objectsAudioSources.Add(newTool);
            newTool.volume = Mathf.Clamp01(volumeSlider.value * objectsVolumeMultiplier);
        }
    }
    public void SetSliderSettings() => mainLight.intensity = spotLights[0].intensity*0.006f;
        
    
}

