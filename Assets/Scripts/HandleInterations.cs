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
    }
    void OnTriggerExit(Collider other)
    {
         if (other.gameObject.CompareTag("Door"))
        other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        messageHandler?.HideMessage();
        Debug.Log("Exited trigger with: " + other.gameObject.name);
    }
}
