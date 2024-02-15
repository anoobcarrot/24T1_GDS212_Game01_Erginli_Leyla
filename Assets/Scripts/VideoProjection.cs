using UnityEngine;
using UnityEngine.Video;

public class VideoProjection : MonoBehaviour
{
    public VideoClip videoClip;
    public GameObject playerObject;
    public float audioRadius = 8f;
    public float fadeSpeed = 1f; // Adjust the fade speed as needed
    public AudioSource audioSource;

    private VideoPlayer videoPlayer;
    private float targetVolume = 0f;

    void Start()
    {
        GameObject videoPlayerObject = new GameObject("VideoPlayer");
        videoPlayerObject.transform.position = transform.position;

        // Add VideoPlayer component
        videoPlayer = videoPlayerObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.clip = videoClip;

        // RenderTexture
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
        videoPlayer.targetTexture = renderTexture;

        // MeshRenderer to display the video
        MeshRenderer renderer = videoPlayerObject.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Unlit/Texture"));
        renderer.material.mainTexture = renderTexture;

        // Play video
        videoPlayer.Play();
    }

    void Update()
    {
        if (playerObject != null)
        {
            // check distance between the player and the VideoProjection object
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

            float targetVolume = Mathf.Clamp01(1f - distanceToPlayer / audioRadius);

            // fade audio out/in
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
        }
    }
}









