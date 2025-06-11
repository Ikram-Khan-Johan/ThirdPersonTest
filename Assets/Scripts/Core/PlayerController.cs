using System;
using UnityEngine;

// This script controls the player's movement and rotation using a CharacterController.

public class PlayerController : MonoBehaviour
{

    Camera mainCamera;

    // [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 0.1f; // Degrees per second
    [SerializeField] private float speedMultiplier = 1.5f; // Multiplier for speed
    [SerializeField] private float moveSpeed = 5f; // Speed of the player movement

    
    [SerializeField] private float groundCheckDistance = 0.2f; // Distance to check for ground
    [SerializeField] private LayerMask groundLayer; // Layer mask for ground detection
    [SerializeField] private GameObject groundCheck; // Force applied when jumping
                                                    
    [SerializeField] private float jumpForce = 5f; // Force applied when jumping

    private bool isRunning = false; // Flag to check if the player is running
    private Rigidbody rb;
    private bool isGrounded;
    private bool isJumping = false; // Flag to check if the player is jumping

    // Input vector for movement
    Vector3 inputVector;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the player object.");
        }

    }
   

    void Update()
    {
        // HandleMovement();
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckDistance, groundLayer);
        // Debug.Log("Is Grounded: " + isGrounded);
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        inputVector = new Vector3(x, 0, z).normalized;


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true; // Set running flag to true
            moveSpeed *= speedMultiplier; // Increase the move speed
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false; // Set running flag to false
            moveSpeed /= speedMultiplier; // Reset the move speed
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Handle jump logic here
            Debug.Log("Jumping");
            isJumping = true; // Set jumping flag to true
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity before jumping
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply an upward force for jumping

        }

    }

    void FixedUpdate()
    {
        // rb.linearVelocity = Vector3.zero; // Reset velocity to prevent unwanted movement
        HandleMovement();
    }

    void HandleMovement()
    {

        // Vector3 move = new Vector3(x, 0, z).normalized;
        if (inputVector.magnitude >= 0.1f)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0; // Keep the forward vector horizontal
            Vector3 cameraRight = mainCamera.transform.right;
            cameraRight.y = 0; // Keep the right vector horizontal
            Vector3 finalMovementDirection = (cameraForward * inputVector.z) + (cameraRight * inputVector.x);
            finalMovementDirection.Normalize(); // Ensure the length is 1
            Vector3 moveVector = finalMovementDirection * moveSpeed * Time.deltaTime; // Adjust for time

            // rb.MovePosition(rb.position + (inputVector * speed * Time.fixedDeltaTime));
            rb.MovePosition(rb.position + moveVector);
            // transform.position += moveVector; // Move the player in the calculated direction

        }
        //         if (!isGrounded) {

        //             if (rb.velocity.y < 0)
        // {
        //             rb.velocity += Vector3.up * -9.81f * Time.deltaTime;
        // }
        //             Debug.Log("Check Value" + rb.velocity.y);
        //          }

        var forward = mainCamera.transform.forward;
        forward.y = 0; // Keep the forward vector horizontal
        forward.Normalize(); // Normalize the forward vector
        var targetRotation = Quaternion.LookRotation(forward);
        // Rotate the player to face the camera's forward direction
        if (forward.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }
    }


}
