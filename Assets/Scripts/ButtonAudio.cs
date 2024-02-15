using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the same game object
        audioSource = GetComponent<AudioSource>();
        // add button click listener for all buttons in scene
        AddButtonClickListeners();
    }

    void AddButtonClickListeners()
    {
        // get all button components in scene including inactive game objects
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();

        // Add a click listener to each Button
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => PlayButtonClickSound());
        }
    }

    void PlayButtonClickSound()
    {
        // Check if an audio clip is assigned
        if (buttonClickSound != null && audioSource != null)
        {
            // play the audio
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}


