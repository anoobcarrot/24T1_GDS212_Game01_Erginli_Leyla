using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SanityBar : MonoBehaviour
{
    [SerializeField] private Image sanityBarImage; // Sanity Bar image
    [SerializeField] private float drainRate = 1.0f; // Rate at which the sanity bar drains per second
    [SerializeField] private float maxSanity = 600.0f; // Maximum sanity value (10 minutes at 1.0 drain rate)
    [SerializeField] private float currentSanity; // Current sanity value

    private bool isPaused; // Flag to indicate whether sanity drain is paused
    private float lastSanityResetTime; // Time when the sanity bar was last reset

    public GameObject player; // Reference to the player GameObject
    public Transform teleportTarget; // The target position to teleport the player to
    public AudioClip failureAudio; // Audio clip for failure
    public GameObject gameOverUI; // Reference to the game over UI GameObject
    public Image fadePanel; // Reference to the UI panel used for fading
    public GameObject taskUI;
    public GameObject sanityBar;

    private AudioSource audioSource; // Reference to the AudioSource component
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script

    private Actions actions;
    public AudioClip heartbeatAudio; // Heartbeat audio clip

    private void Start()
    {
        currentSanity = maxSanity;
        lastSanityResetTime = Time.time; // Initialise the last reset time
        actions = player.GetComponent<Actions>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check if the sanity bar is currently draining and not paused
        if (currentSanity > 0 && !isPaused)
        {
            // Decrease current sanity based on drain rate and time passed
            currentSanity -= drainRate * Time.deltaTime;

            // Update the fill amount of the sanity bar image
            sanityBarImage.fillAmount = currentSanity / maxSanity;

            // Check if sanity has reached zero
            if (currentSanity <= 0)
            {
                actions.Death();
                // Disable player movement when lock panel is opened
                playerMovement.LockPlayerMovement(true);
                // Hide the task UI
                taskUI.SetActive(false);
                sanityBar.SetActive(false);
                // Trigger death sequence if sanity reaches zero
                StartCoroutine(TriggerDeath());
            }
        }
    }

    // reset the sanity bar to full
    public void ResetSanity()
    {
        currentSanity = maxSanity;
        sanityBarImage.fillAmount = 1.0f;
        lastSanityResetTime = Time.time; // Update the last reset time
        StartCoroutine(TriggerHeartbeat());
    }

    // pause sanity drain
    public void PauseSanityDrain()
    {
        isPaused = true;
    }

    // resume sanity drain
    public void ResumeSanityDrain()
    {
        isPaused = false;
    }

    // get the current sanity percentage
    public float GetCurrentSanityPercentage()
    {
        return (currentSanity / maxSanity) * 100f;
    }

    // get the time since the last sanity bar reset
    public float GetSanityBarTimer()
    {
        return Time.time - lastSanityResetTime;
    }

    // Coroutine to trigger death sequence
    private IEnumerator TriggerDeath()
    {
        float fadeDuration = 3.0f;
        float fadeInTimer = 0f;
        float fadeOutTimer = 0f;
        Color originalColor = Color.clear;
        Color targetColor = Color.black;

        // Fade to black
        while (fadeInTimer < fadeDuration)
        {
            fadeInTimer += Time.deltaTime;
            float t = fadeInTimer / fadeDuration;
            Color currentColor = Color.Lerp(originalColor, targetColor, t);
            fadePanel.color = currentColor;
            yield return null;
        }

        // Ensure the panel remains black for a moment
        yield return new WaitForSeconds(1.0f);
        // Teleport the player to the target position
        player.transform.position = teleportTarget.position;
        playerMovement.LockPlayerMovement(false);
        // Lock the mouse cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Fade back to transparent
        while (fadeOutTimer < fadeDuration)
        {
            fadeOutTimer += Time.deltaTime;
            float t = fadeOutTimer / fadeDuration;
            Color currentColor = Color.Lerp(targetColor, originalColor, t);
            fadePanel.color = currentColor;
            yield return null;
        }

        // Play the failure audio
        audioSource.PlayOneShot(failureAudio);

        // Wait for the failure audio to finish playing
        yield return new WaitForSeconds(failureAudio.length);

        // Show the game over UI
        ShowGameOverUI();
    }

    private void ShowGameOverUI()
    {
        // Activate the game over UI
        gameOverUI.SetActive(true);
        // Disable player movement when lock panel is opened
        playerMovement.LockPlayerMovement(true);
        // Show the mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Coroutine to play heartbeat sound
    private IEnumerator TriggerHeartbeat()
    {
        float sanityDecreaseInterval = 60f;
        float lastSanityDecrease = 0f;

        while (true)
        {
            // calculate current decrease in max sanity units
            float currentSanityDecrease = maxSanity - currentSanity;

            // Check if time to play heartbeat sound
            if (currentSanityDecrease - lastSanityDecrease >= sanityDecreaseInterval)
            {
                Debug.Log("Playing heartbeat sound");
                audioSource.PlayOneShot(heartbeatAudio);
                lastSanityDecrease = currentSanityDecrease;
            }

            yield return null;
        }
    }
}

