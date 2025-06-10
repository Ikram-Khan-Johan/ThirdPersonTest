using UnityEngine;

public class HandleInterations : MonoBehaviour
{

    private IMessage messageHandler;

    [System.Obsolete]
    private void Start()
    {
        // Find the GameUIManager in the scene
        messageHandler = FindObjectOfType<GameUIManager>();
        if (messageHandler == null)
        {
            Debug.LogError("GameUIManager not found in the scene.");
        }
    }

    // void FixedUpdate()
    // {

    //     RaycastHit hit;
    //     // Does the ray intersect any objects excluding the player layer
    //     if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10f))

    //     {
    //         Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
    //         Debug.Log("Did Hit" + hit.transform.gameObject.name);
    //         PerfromInteraction(hit.transform.gameObject, hit.distance);
    //     }

    // }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        Debug.Log("Entered trigger with: " + other.gameObject.name);
    }
    private void OnTriggerStay(Collider other)
    {
        // Debug.Log("Trigger stay with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Door"))
        {
            messageHandler?.ShowMessage("Press 'E' to interact with the door.");

            // Debug.Log("Door Collision detected with: " + other.gameObject.name);
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // Check for interaction input
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Key Pressed: E");
                    interactable.Interact();
                    other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
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
            messageHandler?.ShowMessage("Press 'E' to interact with the chest.");
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
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
            messageHandler?.ShowMessage("Press 'E' to interact with the Light.");
            IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
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
        if (other.gameObject.CompareTag("Door"))
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        messageHandler?.HideMessage();
        Debug.Log("Exited trigger with: " + other.gameObject.name);
    }
    


    void PerfromInteraction(GameObject ob, float distance )
    {
         if (ob.CompareTag("Door"))
        {
            if (distance < 7)
            {
                messageHandler?.ShowMessage("Press 'E' to interact with the door.");

            }
            else 
            {
                messageHandler?.HideMessage();
            }
           
            // Debug.Log("Door Collision detected with: " + other.gameObject.name);
            IInteractable interactable = ob.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // Check for interaction input
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Key Pressed: E");
                    interactable.Interact();
                }
                // Debug.Log("Collided with a Door object!");
            }
            else
            {
                Debug.LogWarning("No IInteractable component found on the door.");
            }
        }
        if (ob.CompareTag("Chest"))
        {
            messageHandler?.ShowMessage("Press 'E' to interact with the chest.");
            IInteractable interactable = ob.GetComponent<IInteractable>();
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
        if (ob.CompareTag("Light"))
        {
            messageHandler?.ShowMessage("Press 'E' to interact with the Light.");
            IInteractable interactable = ob.GetComponent<IInteractable>();
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
}
