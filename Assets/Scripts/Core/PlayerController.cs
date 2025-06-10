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
                                                         // Update is called once per frame
    private Rigidbody rb;
    // private bool isGrounded;
    // private float gravity = -9.81f; // Gravity value
    // private float jumpHeight = 2f; // Height of the jump
    // private float rotationSmoothTime = 0.1f; // Time to smooth the rotation
    // private float rotationVelocity; // Used for SmoothDampAngle

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
    void Update()
    {
        // HandleMovement();

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

        // Debug.Log("Player Rotation: " + transform.rotation.eulerAngles);
        // HandleMovement();
        // Debug.Log("Input Vector: " + inputVector);

          
    }
    IInteractable interactable;
    void FixedUpdate()
    {
        rb.linearVelocity = Vector3.zero; // Reset linear velocity to prevent unwanted movement
        HandleMovement();
    }

  void LateUpdate()
    {
        
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
            float moveSpeed = 5f; // Example speed
            Vector3 moveVector = finalMovementDirection * moveSpeed * Time.deltaTime; // Adjust for time

            // rb.MovePosition(rb.position + (inputVector * speed * Time.fixedDeltaTime));
            // rb.MovePosition(rb.position + moveVector);
            transform.position += moveVector; // Move the player in the calculated direction



        }
        
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

    void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Door"))
        //     other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        Debug.Log("Entered trigger with: " + other.gameObject.name);

        if (other.gameObject.CompareTag("movable"))
        {
            Debug.Log("Collision detected with: " + other.gameObject.name);
            var rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply a force to the movable object
                Vector3 forceDirection = other.transform.position - transform.position;
                rb.AddForce(forceDirection.normalized * speed, ForceMode.Impulse);
            }
            // Debug.Log("Collided with a movable object!");
            // Handle collision with movable object
        }
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
