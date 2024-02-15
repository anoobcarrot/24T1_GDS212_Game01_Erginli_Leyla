using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelCompletionManager : MonoBehaviour
{
    public AudioClip gradeAAudio;
    public AudioClip gradeBAudio;
    public AudioClip gradeCAudio;
    public AudioClip gradeDAudio;
    public GameObject gameOverUI;
    public GameObject taskUI;
    public GameObject optionsUI;

    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public OptionsMenu optionsMenu; // Reference to the OptionsMenu script

    private AudioSource audioSource;
    private float startTime;
    private SanityBar sanityBar; // Reference to the SanityBar instance

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sanityBar = FindObjectOfType<SanityBar>(); // Find and store a reference to the SanityBar instance
        FindObjectOfType<LockChecker>().OnLevelCompleted += HandleLevelCompletion;
    }

    public void HandleLevelCompletion()
    {
        optionsUI.SetActive(false);
        Debug.Log("HandleLevelCompletion called.");
        float sanityPercentage = sanityBar.GetCurrentSanityPercentage(); // Get the current sanity percentage from the SanityBar instance

        if (sanityPercentage >= 75f) // Above 75%
        {
            audioSource.PlayOneShot(gradeAAudio);
        }
        else if (sanityPercentage >= 50f) // Between 50% and 75%
        {
            audioSource.PlayOneShot(gradeBAudio);
        }
        else if (sanityPercentage >= 25f) // Between 25% and 50%
        {
            audioSource.PlayOneShot(gradeCAudio);
        }
        else // Below 25%
        {
            audioSource.PlayOneShot(gradeDAudio);
        }

        // Use the completion time to determine the delay for displaying the game over UI
        float maxDelay = Mathf.Max(gradeAAudio.length, gradeBAudio.length, gradeCAudio.length, gradeDAudio.length);
        Invoke("ShowGameOverUI", maxDelay);
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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void SetStartTime(float time)
    {
        startTime = time;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level 1");
    }
}



