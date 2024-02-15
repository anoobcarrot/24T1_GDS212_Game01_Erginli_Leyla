using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonitorInteraction : MonoBehaviour
{
    public GameObject monitorScreen;
    public GameObject monitorObject;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI statusText;
    public GameObject patientFilesUI;
    public GameObject websiteUI; // Reference to the website UI canvas
    public Button[] websiteButtons; // Array of buttons in the website UI
    public string[] websiteTextArray; // Array of text for the website UI
    public string correctPassword = "Power321";
    public float interactionRadius = 3f;

    public PlayerMovement playerMovement; // Reference to the PlayerMovement script

    private bool passwordCorrect = false;

    public TextMeshProUGUI websiteText; // TextMeshProUGUI for website text

    void Update()
    {
        // Check for input to close the monitor UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (monitorScreen.activeSelf)
            {
                CloseMonitorUI();
            }
            else if (patientFilesUI.activeSelf)
            {
                ClosePatientFilesUI();
            }
            else if (websiteUI.activeSelf) // Close website UI if active
            {
                CloseWebsiteUI();
            }
            return; // Exit the method to prevent further interaction checks if the UI is closed
        }

        if (!websiteUI.activeSelf && Input.GetMouseButtonDown(0) && !monitorScreen.activeSelf && !patientFilesUI.activeSelf) // Left click
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == monitorObject)
                {
                    // Check if the distance between the player and the monitor object is within the interaction radius
                    float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                    if (distance <= interactionRadius)
                    {
                        // Player is within interaction radius, show monitor UI
                        ShowMonitorUI();
                    }
                }
            }
        }

        // Check for Enter key press to check password
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (passwordInput.isFocused) // Check if the password input field is focused
            {
                CheckPassword();
            }
        }
    }

    void ShowMonitorUI()
    {
        // Show the monitor UI canvas
        monitorScreen.SetActive(true);
        // lock player movement when canvas open
        playerMovement.LockPlayerMovement(true);
        // Lock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Reset password input field and status text
        ResetPasswordUI();
    }

    void CloseMonitorUI()
    {
        // Show the monitor UI canvas
        monitorScreen.SetActive(false);

        // enable player movement
        playerMovement.LockPlayerMovement(false);
        // Unlock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset password input field and status text
        ResetPasswordUI();
    }

    void ResetPasswordUI()
    {
        passwordInput.text = ""; // Reset password input field
        statusText.text = ""; // Clear status text
    }

    public void CheckPassword()
    {
        string input = passwordInput.text;
        if (input == correctPassword)
        {
            // Correct password, proceed to show patient files UI after a delay
            statusText.text = "Password correct! Access granted.";
            passwordCorrect = true;
            Invoke("ShowPatientFilesUI", 2f);
        }
        else
        {
            // Incorrect password, display error message
            statusText.text = "Incorrect password. Please try again.";
            Invoke("ResetStatusText", 2f); // Invoke method to reset status text after 2 seconds
        }
    }

    void ResetStatusText()
    {
        statusText.text = ""; // Clear the status text
    }

    void ShowPatientFilesUI()
    {
        if (passwordCorrect)
        {
            // Show the patient files UI canvas
            patientFilesUI.SetActive(true);

            // lock player movement when canvas open
            playerMovement.LockPlayerMovement(true);
            // Lock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Hide the monitor UI canvas
            monitorScreen.SetActive(false);
        }
    }

    void ClosePatientFilesUI()
    {
        // Hide the patient files UI canvas
        patientFilesUI.SetActive(false);

        // enable player movement
        playerMovement.LockPlayerMovement(false);
        // Unlock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset password input field and status text
        ResetPasswordUI();
    }

    public void UpdateWebsiteText(int index)
    {
        // Check if the index is within the array bounds
        if (index >= 0 && index < websiteTextArray.Length)
        {
            // Update the website text
            websiteText.text = websiteTextArray[index];
        }
    }

    public void OpenWebsiteUI()
    {
        websiteUI.SetActive(true); // Activate the website UI
        patientFilesUI.SetActive(false);

        // lock player movement when canvas open
        playerMovement.LockPlayerMovement(true);
        // Lock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseWebsiteUI()
    {
        websiteUI.SetActive(false); // Deactivate the website UI

        // Reset website text to nothing
        websiteText.text = "";

        // Reset website page content
        ResetWebsitePage();

        // unlock player movement when canvas closed
        playerMovement.LockPlayerMovement(false);
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ResetWebsitePage()
    {
        // Reset website text to nothing
        websiteText.text = "";
    }
}







