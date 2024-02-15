using UnityEngine;

public class PaperInteraction : MonoBehaviour
{
    public GameObject toDoListUI;
    public GameObject paperObject;
    public float interactionRadius = 3f;

    public PlayerMovement playerMovement; // Reference to the PlayerMovement script

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && toDoListUI.activeSelf)
        {
            CloseToDoList();
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, interactionRadius))
            {
                if (hit.collider.gameObject == paperObject)
                {
                    float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                    if (distance <= interactionRadius)
                    {
                        OpenToDoList();
                    }
                }
            }
        }

        void OpenToDoList()
        {
            toDoListUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // lock player movement when canvas open
            playerMovement.LockPlayerMovement(true);
        }

        void CloseToDoList()
        {
            toDoListUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // lock player movement when canvas open
            playerMovement.LockPlayerMovement(false);
        }
    }
}

