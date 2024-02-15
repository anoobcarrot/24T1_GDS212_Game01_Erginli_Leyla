using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private VideoPlayer firstVideoPlayer;
    [SerializeField] private VideoClip secondVideoClip;
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private CanvasGroup gameTitleCanvasGroup;
    [SerializeField] private CanvasGroup[] buttonCanvasGroups;
    public Image fadePanel;
    private bool isTransitioning = false;

    private void Start()
    {
        firstVideoPlayer.loopPointReached += OnFirstVideoFinished;
        fadePanel.gameObject.SetActive(true);
    }

    private void OnFirstVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(TransitionToSecondVideo());
    }

    private void Update()
    {
        // Check for left mouse button click and ensure first video is playing
        if (Input.GetMouseButtonDown(0) && !isTransitioning && firstVideoPlayer.isPlaying)
        {
            StartCoroutine(TransitionToSecondVideo());
        }
    }

    private IEnumerator TransitionToSecondVideo()
    {
        if (isTransitioning)
            yield break;

        isTransitioning = true;

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

        // Change video clip and play second video
        firstVideoPlayer.clip = secondVideoClip;
        firstVideoPlayer.isLooping = true;
        firstVideoPlayer.Play();

        // Set alpha to 1 for game title and buttons
        gameTitleCanvasGroup.alpha = 1f;
        foreach (var buttonCanvasGroup in buttonCanvasGroups)
        {
            buttonCanvasGroup.alpha = 1f;
        }

        // Fade back to transparent
        while (fadeOutTimer < fadeDuration)
        {
            fadeOutTimer += Time.deltaTime;
            float t = fadeOutTimer / fadeDuration;
            Color currentColor = Color.Lerp(targetColor, originalColor, t);
            fadePanel.color = currentColor;
            yield return null;
        }

        isTransitioning = false;
    }

    public void FadeToBlackAndLoadNextScene()
    {
        fadePanel.gameObject.SetActive(true);
        StartCoroutine(FadeToBlackAndLoadNextSceneCoroutine());
    }

    private IEnumerator FadeToBlackAndLoadNextSceneCoroutine()
    {
        float fadeDuration = 3.0f;
        float elapsedTime = 0f;
        Color originalColor = Color.clear;
        Color targetColor = Color.black;

        // Fade to black
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            Color currentColor = Color.Lerp(originalColor, targetColor, t);
            fadePanel.color = currentColor;
            yield return null;
        }

        // Load the next scene in the build settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}





