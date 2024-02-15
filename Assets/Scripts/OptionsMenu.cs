using System.Collections;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsUI;
    public PlayerMovement playerMovement;
    public GameObject[] otherUIElements; // Array of other UI elements
    public float delayBeforeOpen = 0.3f; // Delay before opening another UI element

    private bool isOptionsMenuOpen = false;
    private Coroutine openOtherUIRoutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CanToggleOptionsMenu())
        {
            ToggleOptionsMenu();
        }

        foreach (GameObject uiElement in otherUIElements)
        {
            if (uiElement.activeSelf)
            {
                // Close the options menu if it's open
                if (isOptionsMenuOpen)
                {
                    optionsUI.SetActive(false);
                }
                return; // Exit the loop early if any UI is active
            }
        }
    }

    private bool CanToggleOptionsMenu()
    {
        foreach (GameObject uiElement in otherUIElements)
        {
            if (uiElement.activeSelf)
            {
                return false; // If any other UI element is active, cannot toggle options menu
            }
        }
        return true; // If no other UI element is active, can toggle options menu
    }

    private void ToggleOptionsMenu()
    {
        isOptionsMenuOpen = !isOptionsMenuOpen;
        optionsUI.SetActive(isOptionsMenuOpen);

        if (isOptionsMenuOpen)
        {
            
            playerMovement.LockPlayerMovement(true); // Freeze player movement
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true; // Make cursor visible
        }
        else
        {
            
            playerMovement.LockPlayerMovement(false); // Unfreeze player movement
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor
            Cursor.visible = false; // Hide cursor
        }
    }

    // Coroutine to open another UI element after a delay
    private IEnumerator OpenOtherUIAfterDelay(GameObject uiElement)
    {
        yield return new WaitForSeconds(delayBeforeOpen);

        // Disable options menu before opening another UI element
        if (isOptionsMenuOpen)
        {
            ToggleOptionsMenu();
        }

        // Open the specified UI element
        uiElement.SetActive(true);
    }

    // Method to start opening another UI element
    public void OpenOtherUIWithDelay(GameObject uiElement)
    {
        // Cancel any existing coroutine
        if (openOtherUIRoutine != null)
        {
            StopCoroutine(openOtherUIRoutine);
        }

        // Start the coroutine to open the specified UI element after a delay
        openOtherUIRoutine = StartCoroutine(OpenOtherUIAfterDelay(uiElement));
    }
}



