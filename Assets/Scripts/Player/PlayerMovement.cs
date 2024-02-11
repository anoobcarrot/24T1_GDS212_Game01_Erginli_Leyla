using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeedMultiplier = 2f;
    public float crouchSpeedMultiplier = 0.5f;
    public float jumpForce = 10f;
    public float gravity = -2f;
    public float lookSensitivity = 2f;

    public Animator playerAnimator; // Reference to the player's Animator component
    public Transform playerBody; // Reference to the player's body or root GameObject
    public Transform cameraTransform; // Reference to the player's camera transform

    private Rigidbody rb;
    private Actions actions;
    private bool isGrounded;

    private float verticalRotation = 0f; // Current vertical rotation of the camera

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        actions = GetComponent<Actions>(); // Get the Actions component
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the player is grounded when colliding with objects tagged as ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the player is not grounded when exiting collision with objects tagged as ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private bool wasMoving = false; // Flag to track if the player was previously moving

    private void Update()
    {
        // Perform a raycast downwards to check if the player is grounded
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f)) // Adjust the raycast distance as needed
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Check if there is movement input
        bool isMoving = Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0;

        // Check if the player is grounded for movement and jumping
        if (isGrounded)
        {
            // Check if the player is moving or attempting to jump
            if (isMoving || Input.GetKeyDown(KeyCode.Space))
            {
                // Trigger walk animation if moving, otherwise trigger idle animation
                if (isMoving)
                {
                    actions.Walk();
                    wasMoving = true; // Set the flag indicating the player was moving
                }
                else
                {
                    actions.Stay();
                    wasMoving = false; // Reset the flag since the player is not moving
                }

                // Apply movement speed
                float currentSpeed = walkSpeed;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    actions.Run();
                    currentSpeed *= runSpeedMultiplier;
                }

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    currentSpeed *= crouchSpeedMultiplier;
                }

                // Apply movement
                Vector3 moveDirection = playerBody.right * horizontalInput + playerBody.forward * verticalInput;
                Vector3 velocity = moveDirection.normalized * currentSpeed;
                velocity.y = rb.velocity.y; // Preserve vertical velocity
                rb.velocity = velocity;

                // Jump
                if (Input.GetKeyDown(KeyCode.Space)) // Only allow jumping when grounded
                {
                    actions.Jump();
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }
            else if (wasMoving) // Check if the player was previously moving
            {
                // Player stopped moving, trigger idle animation
                actions.Stay();
                wasMoving = false; // Reset the flag
            }

            // Mouse look
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

            // Rotate the player's body left/right
            playerBody.Rotate(Vector3.up * mouseX);

            // Rotate the camera vertically
            RotateCameraVertical(mouseY);
        }
        else
        {
            // Player is not grounded
            actions.Stay(); // Trigger idle animation
        }

        // Apply gravity
        rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
    }




    private void RotateCameraVertical(float mouseY)
    {
        // Rotate the camera vertically
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -53f, 90f);

        Vector3 currentRotation = cameraTransform.localEulerAngles;
        currentRotation.x = verticalRotation;
        cameraTransform.localEulerAngles = currentRotation;
    }
}










