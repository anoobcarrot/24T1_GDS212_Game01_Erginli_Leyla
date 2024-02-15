using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LockChecker : MonoBehaviour
{
    public ImageSlot[] imageSlots;
    public Button checkButton;
    public AudioClip correctAudio;
    public AudioClip wrongAudio;
    public GameObject taskUI;
    public GameObject sanityBarImage;

    public SanityBar sanityBar;

    public LockSystem lockSystem;
    public delegate void LevelCompletedAction(); // Define a delegate for level completion
    public event LevelCompletedAction OnLevelCompleted; // Event for level completion

    // Array to store the correct image order
    public Sprite[] correctOrder;

    public Transform teleportTarget; // The target position to teleport the player to
    public GameObject player; // Reference to the player GameObject

    public GameObject canvasGroupGameObject; // Reference to the GameObject containing the CanvasGroup component

    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();
    }

    public void CheckImageOrder()
    {
        bool isCorrect = CheckImageOrderCorrectness();

        if (isCorrect)
        {
            // Play the correct audio clip using the AudioSource component
            audioSource.PlayOneShot(correctAudio);
            StartCoroutine(CompleteLevel());
        }
        else
        {
            // Play the wrong audio clip using the AudioSource component
            audioSource.PlayOneShot(wrongAudio);
        }
    }

    private bool CheckImageOrderCorrectness()
    {
        // Check if the number of image slots matches the correct order
        if (imageSlots.Length != correctOrder.Length)
        {
            return false;
        }

        // Compare the images in the image slots with the correct order
        for (int i = 0; i < imageSlots.Length; i++)
        {
            if (imageSlots[i].image.sprite != correctOrder[i])
            {
                return false; // Incorrect order
            }
        }

        return true; // Correct order
    }

    private IEnumerator CompleteLevel()
    {
        lockSystem.CloseLockPanel();
        // Pause sanity drain
        sanityBar.PauseSanityDrain();

        sanityBarImage.SetActive(false);
        // Hide the task UI
        taskUI.SetActive(false);
        // Fade out
        float fadeDuration = 2.0f;
        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            // Do fade out logic
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        // Teleport the player to the target position
        player.transform.position = teleportTarget.position;

        // Fade in
        fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            // Do fade in logic
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        // Disable the GameObject containing the CanvasGroup component
        canvasGroupGameObject.SetActive(false);

        // Call HandleLevelCompletion method after fading in
        FindObjectOfType<LevelCompletionManager>().HandleLevelCompletion();
    }
}






