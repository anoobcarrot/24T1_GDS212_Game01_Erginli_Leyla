using UnityEngine;
using System.Collections;

public class EvaVoice : MonoBehaviour
{
    public AudioClip firstVoiceClip;
    public AudioClip secondVoiceClip;
    public GameObject wallToDestroy;
    public GameObject sanityBar;
    public string newTaskDescription;

    [SerializeField] private TaskManager taskManager;
    private AudioSource audioSource;

    private void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
        audioSource = GetComponent<AudioSource>();

        // Play the first voice clip
        PlayFirstVoice();
    }

    private void PlayFirstVoice()
    {
        // Assign the first voice clip to the AudioSource and play it
        audioSource.clip = firstVoiceClip;
        audioSource.Play();
    }

    public void CompleteFirstTasks()
    {
        // Stop any voice playback and play the second voice clip
        audioSource.Stop();
        StartCoroutine(PlaySecondVoice());
    }

    private IEnumerator PlaySecondVoice()
    {
        // Wait for a short delay before playing the second voice clip
        yield return new WaitForSeconds(1f);

        // Assign the second voice clip to the AudioSource and play it
        audioSource.clip = secondVoiceClip;
        audioSource.Play();

        // Wait for the second voice clip playback to finish
        yield return new WaitForSeconds(secondVoiceClip.length);

        // Perform other actions after the second voice clip finishes
        Destroy(wallToDestroy);
        sanityBar.SetActive(true);

        // Update the task list to "Unlock the Door"
        taskManager.UpdateLevel1Task();
    }
}



