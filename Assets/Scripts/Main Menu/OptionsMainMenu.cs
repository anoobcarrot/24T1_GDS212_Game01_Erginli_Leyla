using UnityEngine;

public class OptionsMainMenu : MonoBehaviour
{
    public GameObject optionsUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableOptionsMenu();
        }
    }

    public void EnableOptionsMenu()
    {
        optionsUI.SetActive(true);
    }

    public void DisableOptionsMenu()
    {
        optionsUI.SetActive(false);
    }
}

