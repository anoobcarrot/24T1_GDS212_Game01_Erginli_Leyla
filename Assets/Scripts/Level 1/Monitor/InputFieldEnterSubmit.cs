using UnityEngine;
using TMPro;

public class InputFieldEnterSubmit : MonoBehaviour
{
    public TMP_InputField inputField;
    public MonitorInteraction monitorInteraction; // MonitorInteraction script

    private void Start()
    {
        // Subscribe to the onEndEdit event of the input field
        inputField.onSubmit.AddListener(SubmitPassword);
    }

    private void SubmitPassword(string password)
    {
        // check if le password is submitted via Enter key
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // submit password
            monitorInteraction.CheckPassword();
        }
    }
}

