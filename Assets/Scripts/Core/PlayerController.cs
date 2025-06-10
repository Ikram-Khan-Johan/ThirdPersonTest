using System;
using Unity.VisualScripting;
using UnityEngine;

// This script controls the player's movement and rotation using a CharacterController.

public class PlayerController : MonoBehaviour
{

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
       

    }
    IInteractable interactable;
    void FixedUpdate()
    {
        HandleMovement();
    }
    void HandleMovement()
    {

        // Vector3 move = new Vector3(x, 0, z).normalized;
        if (inputVector.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + (inputVector * speed * Time.fixedDeltaTime));
            // Calculate the target angle based on the input direction
            float targetAngle = Mathf.Atan2(inputVector.x, inputVector.z) * Mathf.Rad2Deg;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            // Rotate the player to face the direction of movement
            transform.rotation = Quaternion.Euler(0, angle, 0);
            // Move the character controller in the specified direction
            // Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            // rb.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);
            // characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    }



    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Collision detected with: " + collision.gameObject.name);
    //     if (collision.gameObject.CompareTag("movable"))
    //     {
    //         var rb = collision.gameObject.GetComponent<Rigidbody>();
    //         if (rb != null)
    //         {
    //             // Apply a force to the movable object
    //             Vector3 forceDirection = collision.transform.position - transform.position;
    //             rb.AddForce(forceDirection.normalized * speed, ForceMode.Impulse);
    //         }
    //         Debug.Log("Collided with a movable object!");
    //         // Handle collision with movable object
    //     }

    // }
    // void OnControllerColliderHit(ControllerColliderHit hit)
    // {

    //     if (hit.gameObject.CompareTag("movable"))
    //     {
    //         Debug.Log("Collision detected with: " + hit.gameObject.name);
    //         var rb = hit.gameObject.GetComponent<Rigidbody>();
    //         if (rb != null)
    //         {
    //             // Apply a force to the movable object
    //             Vector3 forceDirection = hit.transform.position - transform.position;
    //             rb.AddForce(forceDirection.normalized * speed, ForceMode.Impulse);
    //         }
    //         // Debug.Log("Collided with a movable object!");
    //         // Handle collision with movable object
    //     }
    //     if (hit.gameObject.CompareTag("CollectableCoin"))
    //     {
    //         Debug.Log("Collision detected with: " + hit.gameObject.name);
    //         ISoundPlayer soundPlayer = hit.gameObject.GetComponent<ISoundPlayer>();
    //         if (soundPlayer != null)
    //         {
    //             AudioClip audioClip = Resources.Load<AudioClip>("collect_coin");
    //             soundPlayer.PlaySound(audioClip);
    //             hit.gameObject.GetComponent<MeshRenderer>().enabled = false; // Hide the coin mesh
    //             hit.gameObject.GetComponent<Collider>().enabled = false;
    //             Destroy(hit.gameObject, 1f); // Destroy the coin after 1 second to allow sound to play
    //         }
    //         else
    //         {
    //             Debug.LogWarning("No ISoundPlayer component found on the coin.");
    //         }
    //     }

    // }

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
