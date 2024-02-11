using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private VideoPlayer firstVideoPlayer;
    [SerializeField] private VideoClip secondVideoClip;
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private CanvasGroup gameTitleCanvasGroup;
    [SerializeField] private CanvasGroup[] buttonCanvasGroups;

    private bool isTransitioning = false;

    private void Start()
    {
        firstVideoPlayer.loopPointReached += OnFirstVideoFinished;
    }

    private void OnFirstVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(TransitionToSecondVideo());
    }

    private IEnumerator TransitionToSecondVideo()
    {
        if (isTransitioning)
            yield break;

        isTransitioning = true;

        // Fade to black
        float fadeDuration = 1.0f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            blackScreen.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

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

        // Fade back from black
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            blackScreen.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isTransitioning = false;
    }
}




