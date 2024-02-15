using UnityEngine;

public class DraggableDoor : MonoBehaviour
{
    private Vector3 initialMousePosition;
    private Quaternion initialDoorRotation;

    private bool isDragging = false;

    [SerializeField] private float rotationSpeed = 50f;

    void Start()
    {
        // Store initial rotation of the door
        initialDoorRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if mouse is over the door
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // Start dragging
                isDragging = true;
                initialMousePosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Stop dragging
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            // Calculate door rotation based on mouse movement
            Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
            float rotationAmount = mouseDelta.x * rotationSpeed * Time.deltaTime;
            Quaternion doorRotation = Quaternion.AngleAxis(rotationAmount, Vector3.up) * initialDoorRotation;

            // Freeze X and Z rotation
            doorRotation.eulerAngles = new Vector3(0f, doorRotation.eulerAngles.y, 0f);

            // Apply door rotation
            transform.rotation = doorRotation;
        }
    }
}





