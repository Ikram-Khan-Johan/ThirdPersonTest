using System;
using Unity.VisualScripting;
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

    private bool isRunning = false; // Flag to check if the player is running
    private Rigidbody rb;
     private bool isGrounded;
     [SerializeField] private float groundCheckDistance = 0.2f; // Distance to check for ground
     [SerializeField] private LayerMask groundLayer; // Layer mask for ground detection
    [SerializeField] private GameObject groundCheck; // Force applied when jumping
                                                     // private float gravity = -9.81f; // Gravity value
                                                     // private float jumpHeight = 2f; // Height of the jump
    [SerializeField] private float jumpForce = 5f; // Force applied when jumping
    
private bool isJumping = false; // Flag to check if the player is jumping

    // Input vector for movement
    private IMessage messageHandler;

    [Obsolete]
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the player object.");
        }
        messageHandler = FindObjectOfType<GameUIManager>();
        if (messageHandler == null)
        {
            Debug.LogError("GameUIManager not found in the scene.");
        }
    }
    Vector3 inputVector;

    bool inRange = false;
    IInteractable interactable;
    void Update()
    {
        // HandleMovement();
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckDistance, groundLayer);
        // Debug.Log("Is Grounded: " + isGrounded);
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        inputVector = new Vector3(x, 0, z).normalized;

        if (Input.GetKeyDown(KeyCode.E) && inRange)
        {
            // Check if the player is in range to interact with an object
            Debug.Log("Key Pressed: E");
            interactable.Interact();
            // Handle interaction logic here
        }
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

        // if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        // {
        //     // Handle jump logic here
        //     Debug.Log("Jumping");
        //     isJumping = true; // Set jumping flag to true
        //     rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity before jumping
        //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply an upward force for jumping

        // }
        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector3.zero; // Reset linear velocity to prevent unwanted movement
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
            // rb.MovePosition(rb.position + moveVector);
            transform.position += moveVector; // Move the player in the calculated direction

        }
//         if (!isGrounded) {

//             if (rb.linearVelocity.y < 0)
// {
//             rb.linearVelocity += Vector3.up * -9.81f * Time.deltaTime;
// }
//             Debug.Log("Check Value" + rb.linearVelocity.y);
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

    void OnCollisionEnter(Collision collision)
    {
        
         if (collision.gameObject.CompareTag("movable"))
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply a force to the movable object
                Vector3 forceDirection = collision.transform.position - transform.position;
                rb.AddForce(forceDirection.normalized * 100, ForceMode.Impulse);
                Debug.Log("Applied force to movable object: " + collision.gameObject.name + " with force: " + forceDirection.normalized * 100);
            }
            // Debug.Log("Collided with a movable object!");
            // Handle collision with movable object
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Door"))
        //     other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        // Debug.Log("Entered trigger with: " + other.gameObject.name);

       if (other.gameObject.CompareTag("CollectableCoin"))
        {
            Debug.Log("Collision detected with: " + other.gameObject.name);
            ISoundPlayer soundPlayer = other.gameObject.GetComponent<ISoundPlayer>();
            if (soundPlayer != null)
            {
                AudioClip audioClip = Resources.Load<AudioClip>("collect_coin");
                soundPlayer.PlaySound(audioClip);
                other.gameObject.GetComponent<MeshRenderer>().enabled = false; // Hide the coin mesh
                other.gameObject.GetComponent<Collider>().enabled = false;
                Destroy(other.gameObject, 1f); // Destroy the coin after 1 second to allow sound to play
            }
            else
            {
                Debug.LogWarning("No ISoundPlayer component found on the coin.");
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (inRange) return;
        // Prevents multiple messages if already in range
        Debug.Log("Trigger stay with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Door"))
        {
            messageHandler?.ShowMessage("Press 'E' to interact with the door.");
            inRange = true;
            // Debug.Log("Door Collision detected with: " + other.gameObject.name);
            interactable = other.gameObject.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                // Check for interaction input
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Key Pressed: E");
                    interactable.Interact();
                    other.gameObject.GetComponentInParent<BoxCollider>().isTrigger = true;
                }
                // Debug.Log("Collided with a Door object!");
            }
            else
            {
                Debug.LogWarning("No IInteractable component found on the door.");
            }
        }
        if (other.gameObject.CompareTag("Chest"))
        {
            inRange = true;
            messageHandler?.ShowMessage("Press 'E' to interact with the chest.");
            interactable = other.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // Check for interaction input
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Key Pressed: E");
                    interactable.Interact();
                }
            }
            else
            {
                Debug.LogWarning("No IInteractable component found on the chest.");
            }
        }
        if (other.gameObject.CompareTag("Light"))
        {
            inRange = true;
            messageHandler?.ShowMessage("Press 'E' to interact with the Light.");
            interactable = other.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // Check for interaction input
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Key Pressed: E");
                    interactable.Interact();
                }
            }
            else
            {
                Debug.LogWarning("No IInteractable component found on the NPC.");
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        inRange = false;
        // if (other.gameObject.CompareTag("Door"))
        //     other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        messageHandler?.HideMessage();
        Debug.Log("Exited trigger with: " + other.gameObject.name);
    }
}
