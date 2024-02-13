using UnityEngine;

public class LockSystem : MonoBehaviour
{
    public GameObject lockPanel;
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public GameObject lockObject; // Reference to the lock GameObject

    private Camera mainCamera;

    private void Start()
    {
        lockPanel.SetActive(false);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Cast a ray from the main camera's position forward
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits the lock object
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == lockObject)
            {
                // Check for mouse click to interact
                if (Input.GetMouseButtonDown(0))
                {
                    OpenLockPanel();
                }
            }
        }
    }

    public void OpenLockPanel()
    {
        lockPanel.SetActive(true);
        // Disable player movement when lock panel is opened
        playerMovement.LockPlayerMovement(true);
        // Show the mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Method to close the lock panel and enable player movement
    public void CloseLockPanel()
    {
        lockPanel.SetActive(false);
        // Enable player movement when lock panel is closed
        playerMovement.LockPlayerMovement(false);
        // Lock the mouse cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}





