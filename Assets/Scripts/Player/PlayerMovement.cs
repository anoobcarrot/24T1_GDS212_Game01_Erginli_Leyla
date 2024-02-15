using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeedMultiplier = 2f;
    public float jumpForce = 10f;
    public float gravity = -2f;
    public float lookSensitivity = 2f;

    public OptionsManager optionsManager;

    public CapsuleCollider playerCollider;
    public Animator playerAnimator;
    public Transform playerBody;
    public Transform cameraTransform;
    public string obstacleTag = "Ceiling";

    public AudioClip footstepSound; // Footstep audio clip
    public AudioSource audioSource; // AudioSource for playing footstep sound

    private Rigidbody rb;
    private Actions actions;
    private bool isGrounded;
    private bool isCrouching = false;
    private bool isLocked = false;
    private float verticalRotation = 0f;
    private Vector3 originalCameraPosition;
    private float originalColliderHeight;
    private float originalColliderCenterY;
    private float crouchSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        actions = GetComponent<Actions>();
        Cursor.lockState = CursorLockMode.Locked;

        originalCameraPosition = cameraTransform.localPosition;
        originalColliderHeight = playerCollider.height;
        originalColliderCenterY = playerCollider.center.y;

        // Calculate crouch speed as half of the walk speed
        crouchSpeed = walkSpeed * 0.5f;
    }

    public void UpdateLookSensitivity(float sensitivity)
    {
        lookSensitivity = sensitivity;
    }

    public bool IsMovementLocked
    {
        get { return isLocked; }
    }

    private void Update()
    {
        if (isLocked)
            return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0;

        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        playerBody.Rotate(Vector3.up * mouseX);
        RotateCameraVertical(mouseY);

        // Reset velocity if not moving
        if (!isMoving && isGrounded)
        {
            rb.velocity = Vector3.zero;
        }

        if (isGrounded)
        {
            if (isMoving || Input.GetKeyDown(KeyCode.Space))
            {
                if (isMoving)
                    actions.Walk();
                else
                    actions.Stay();

                float currentSpeed = walkSpeed;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    actions.Walk();
                    currentSpeed *= runSpeedMultiplier;
                }

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    currentSpeed = crouchSpeed;
                    isCrouching = true;
                    Crouch();
                }
                else if (isCrouching && !IsObstacleAbove())
                {
                    isCrouching = false;
                    Uncrouch();
                }

                if (playerCollider.height == originalColliderHeight / 2f)
                {
                    currentSpeed = crouchSpeed;
                }

                Vector3 moveDirection = playerBody.right * horizontalInput + playerBody.forward * verticalInput;
                Vector3 velocity = moveDirection.normalized * currentSpeed;
                velocity.y = rb.velocity.y;
                rb.velocity = velocity;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    actions.Walk();
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }
            else
            {
                actions.Stay();

                // Stop footstep sound when not moving
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }

            playerBody.Rotate(Vector3.up * mouseX);
            RotateCameraVertical(mouseY);
        }
        else
        {
            actions.Stay();
        }

        rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);

        // Play footstep sound if moving and not crouching
        if (isMoving && !isCrouching && footstepSound && audioSource && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(footstepSound);
        }
    }

    private void RotateCameraVertical(float mouseY)
    {
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 53f);

        Vector3 currentRotation = cameraTransform.localEulerAngles;
        currentRotation.x = verticalRotation;
        cameraTransform.localEulerAngles = currentRotation;
    }

    private void Crouch()
    {
        cameraTransform.localPosition = originalCameraPosition / 2f;

        if (playerCollider.height != originalColliderHeight / 2f)
        {
            playerCollider.height = originalColliderHeight / 2f;
            playerCollider.center = new Vector3(playerCollider.center.x, originalColliderCenterY / 2f, playerCollider.center.z);
        }
    }

    private void Uncrouch()
    {
        cameraTransform.localPosition = originalCameraPosition;

        if (playerCollider.height != originalColliderHeight)
        {
            playerCollider.height = originalColliderHeight;
            playerCollider.center = new Vector3(playerCollider.center.x, originalColliderCenterY, playerCollider.center.z);
        }
    }

    private bool IsObstacleAbove()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f); // check colliders within 1.5 units of the player
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag(obstacleTag))
            {
                return true;
            }
        }
        return false;
    }

    public void LockPlayerMovement(bool lockMovement)
    {
        isLocked = lockMovement;
        Cursor.visible = lockMovement;

        if (lockMovement)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
