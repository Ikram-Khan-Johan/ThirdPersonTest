using UnityEngine;

public class HandleInterations : MonoBehaviour
{

    private IMessage messageHandler;

    private IScoreManager scoreManager;

    bool inRange = false;
    IInteractable interactable;
    [System.Obsolete]
    private void Start()
    {
        // Find the GameUIManager in the scene
        messageHandler = FindObjectOfType<GameUIManager>();
        if (messageHandler == null)
        {
            Debug.LogError("GameUIManager not found in the scene.");
        }
         messageHandler = FindObjectOfType<GameUIManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        if (messageHandler == null)
        {
            Debug.LogError("GameUIManager not found in the scene.");
        }

    }

    void Update()
    {
          if (Input.GetKeyDown(KeyCode.E) && inRange)
        {
            // Check if the player is in range to interact with an object
            // Debug.Log("Key Pressed: E");
            interactable.Interact();
            // Handle interaction logic here
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("movable"))
        {
            // Debug.Log("Collision detected with: " + collision.gameObject.name);
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply a force to the movable object
                Vector3 forceDirection = collision.transform.position - transform.position;
                rb.AddForce(forceDirection.normalized * 50, ForceMode.Impulse);
                // Debug.Log("Applied force to movable object: " + collision.gameObject.name + " with force: " + forceDirection.normalized * 100);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
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
                scoreManager?.AddScore(1); // Increment the score by 1
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
           
        }
        if (other.gameObject.CompareTag("Chest"))
        {
            inRange = true;
            messageHandler?.ShowMessage("Press 'E' to interact with the chest.");
            interactable = other.gameObject.GetComponent<IInteractable>();
            
        }
        if (other.gameObject.CompareTag("Light"))
        {
            inRange = true;
            messageHandler?.ShowMessage("Press 'E' to interact with the Light.");
            interactable = other.gameObject.GetComponent<IInteractable>();
           
        }
    }
    void OnTriggerExit(Collider other)
    {
        inRange = false;
        // if (other.gameObject.CompareTag("Door"))
        //     other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        messageHandler?.HideMessage();
        Debug.Log("Exited trigger with: " + other.gameObject.name);
        StopAllCoroutines(); // Stop any running coroutines when exiting the trigger
    }


}
