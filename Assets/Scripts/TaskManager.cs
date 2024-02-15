using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TaskManager : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    public delegate void LevelCompletedAction(); // Define a delegate for level completion
    public event LevelCompletedAction OnLevelCompleted; // Event for level completion

    private bool hasMoved;
    private bool hasJumped;
    private bool hasSprinted;
    private bool hasCrouched;

    private bool areTasksShown; // Flag to track if tasks are shown

    public SanityBar sanityBar;
    public EvaVoice evaVoice; // Reference to the EvaVoice script

    private void Start()
    {
        taskText.gameObject.SetActive(false);

        StartCoroutine(StartVoicePlayback());
    }

    IEnumerator StartVoicePlayback()
    {
        sanityBar.PauseSanityDrain();
        // Wait for voice playback to finish
        yield return new WaitForSeconds(18f);

        // Enable the task UI after voice playback
        taskText.gameObject.SetActive(true);
        UpdateTaskList("Move around with WASD");

        areTasksShown = true; // Set tasks shown flag
    }

    private void Update()
    {
        if (!areTasksShown)
            return; // Don't update task completion if tasks are not shown yet

        // Check for movement input
        if (!hasMoved && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            TaskCompleted("Move around with WASD");
        }

        // Check for jump input
        if (!hasJumped && Input.GetKeyDown(KeyCode.Space))
        {
            TaskCompleted("Jump with Spacebar");
        }

        // Check for sprint input
        if (!hasSprinted && Input.GetKey(KeyCode.LeftShift))
        {
            TaskCompleted("Sprint with Left Shift");
        }

        // Check for crouch input
        if (!hasCrouched && Input.GetKey(KeyCode.LeftControl))
        {
            TaskCompleted("Crouch with Left Control");
        }
    }

    public void UpdateTaskList(string newTask)
    {
        taskText.text = newTask;
    }

    public void TaskCompleted(string task)
    {
        switch (task)
        {
            case "Move around with WASD":
                if (taskText.text == "Move around with WASD")
                {
                    hasMoved = true;
                    UpdateTaskList("Jump with Spacebar");
                }
                break;
            case "Jump with Spacebar":
                if (taskText.text == "Jump with Spacebar")
                {
                    hasJumped = true;
                    UpdateTaskList("Sprint with Left Shift");
                }
                break;
            case "Sprint with Left Shift":
                if (taskText.text == "Sprint with Left Shift")
                {
                    hasSprinted = true;
                    UpdateTaskList("Crouch with Left Control");
                }
                break;
            case "Crouch with Left Control":
                if (taskText.text == "Crouch with Left Control")
                {
                    hasCrouched = true;
                    // All tasks completed, display completion message
                    ShowCompletionMessage();
                }
                break;
            default:
                break;
        }
    }

    private void ShowCompletionMessage()
    {
        taskText.gameObject.SetActive(true);
        taskText.text = "";
        // Call CompleteFirstTasks from EvaVoice script
        evaVoice.CompleteFirstTasks();
    }

    // Public method to update task to "Unlock the Door"
    public void UpdateLevel1Task()
    {
        taskText.text = "Unlock the Door \nHint: You can interact with objects around with left click";
        sanityBar.ResumeSanityDrain();
        sanityBar.ResetSanity();
    }
}






