using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public PlayerMovement playerMovement;

    public AudioSource[] allAudioSources; // Assign the audio sources in the inspector

    private void Start()
    {

        // Set the volume slider value
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;

        // Adjust the volume based on the loaded settings
        AdjustVolume();

        // Set the sensitivity slider value
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        sensitivitySlider.value = savedSensitivity;

        // Adjust volume and mouse sensitivity based on loaded settings
        AdjustMouseSensitivity();
    }

    public void AdjustVolume()
    {
        float volume = volumeSlider.value;
        Debug.Log("Volume Adjusted: " + volume);

        // Adjust volume for all assigned audio sources
        foreach (var audioSource in allAudioSources)
        {
            audioSource.volume = volume;
        }

        // Save volume
        PlayerPrefs.SetFloat("Volume", volume);
    }


    public void AdjustMouseSensitivity()
    {
        float sensitivity = sensitivitySlider.value;
        playerMovement.UpdateLookSensitivity(sensitivity); // Update player's look sensitivity
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
    }

    public void ApplyChanges()
    {
        // Save settings when apply button is clicked
        PlayerPrefs.Save();
    }
}

