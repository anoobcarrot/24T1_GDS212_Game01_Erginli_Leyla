using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    public Canvas bookCanvas;
    public float interactionRadius = 5f;

    private bool canvasOpen = false;

    public PlayerMovement playerMovement; // Reference to the PlayerMovement script

    void Update()
    {
        // Check if the canvas is open and the player presses the Escape key
        if (canvasOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCanvas();
        }
    }

    void OnMouseDown()
    {
        // Calculate the distance between the player and the book object
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        // Check if the player is within the interaction radius
        if (distance <= interactionRadius)
        {
            // Toggle canvas visibility
            if (canvasOpen)
            {
                CloseCanvas();
            }
            else
            {
                OpenCanvas();
            }
        }
    }

    void OpenCanvas()
    {
        // Activate the canvas
        bookCanvas.gameObject.SetActive(true);
        canvasOpen = true;

        // lock player movement when canvas open
        playerMovement.LockPlayerMovement(true);
        // Lock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseCanvas()
    {
        // Deactivate the canvas
        bookCanvas.gameObject.SetActive(false);
        canvasOpen = false;

        // enable player movement
        playerMovement.LockPlayerMovement(false);
        // Unlock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

