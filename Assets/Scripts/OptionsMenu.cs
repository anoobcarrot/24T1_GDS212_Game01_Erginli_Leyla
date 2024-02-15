using System.Collections;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsUI;
    public PlayerMovement playerMovement;
    public GameObject[] otherUIElements; // Array of other UI elements
    public float delayBeforeToggle = 0.3f; // Delay before toggling options menu again
    public float timeBeforeOptionsActivation = 3f; // Time before options UI can be activated after other UI elements are deactivated

    private bool isOptionsMenuOpen = false;
    private bool canToggleOptions = true; // Flag to track if options UI can be toggled
    private Coroutine toggleOptionsMenuRoutine;
    private float timeUIDeactivated; // Time when other UI elements became deactivated

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CanToggleOptionsMenu())
        {
            ToggleOptionsMenuWithDelay();
        }
    }

    private bool CanToggleOptionsMenu()
    {
        // Allow toggling options menu only if the flag is true and the specified time has passed since other UI elements became deactivated
        return canToggleOptions && (Time.time - timeUIDeactivated) >= timeBeforeOptionsActivation;
    }

    private void ToggleOptionsMenuWithDelay()
    {
        if (toggleOptionsMenuRoutine == null)
        {
            toggleOptionsMenuRoutine = StartCoroutine(ToggleOptionsMenuAfterDelay());
        }
    }

    private IEnumerator ToggleOptionsMenuAfterDelay()
    {
        // Set flag to prevent options UI activation for a specified duration
        canToggleOptions = false;
        yield return new WaitForSeconds(delayBeforeToggle);

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

        toggleOptionsMenuRoutine = null; // Reset coroutine reference
        canToggleOptions = true; // Allow toggling options UI again
    }

    private void FixedUpdate()
    {
        // Update the time when other UI elements became deactivated
        UpdateTimeUIDeactivated();
    }

    private void UpdateTimeUIDeactivated()
    {
        foreach (GameObject uiElement in otherUIElements)
        {
            if (uiElement.activeSelf)
            {
                timeUIDeactivated = Time.time;
                return; // Exit the loop early if any UI is active
            }
        }
    }
}









