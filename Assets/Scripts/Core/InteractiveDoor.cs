using UnityEngine;

public class InteractiveDoor : MonoBehaviour, IInteractable
{
    // This script implements the IInteractable interface for a door object.
    // It allows interaction with the door, such as opening or closing it.

    [SerializeField] private Animator animator;
    bool isOpen = false;
    public void Interact()
    {
        // Implement the interaction logic here
        Debug.Log("Interacted with door by: " );
        // For example, toggle the door's open/close state
        ToggleDoor();
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         ToggleDoor();
    //     }
    // }
    private void ToggleDoor()
    {
        ToggleDoor(animator);
    }

    private void ToggleDoor(Animator animator)
    {
      if (animator.GetBool("isOpen"))
        {
            animator.SetBool("isOpen", false);
            isOpen = false;
            GetComponent<BoxCollider>().isTrigger = false;
            Debug.Log("Door closed.");
        }
        else
        {
            animator.SetBool("isOpen", true);
            isOpen = true;
            GetComponent<BoxCollider>().isTrigger = true;
            Debug.Log("Door opened.");
        }
        // Logic to toggle the door's state (open/close)
        Debug.Log("Toggling door state.");
        // You can add animations or sound effects here
    }
}
