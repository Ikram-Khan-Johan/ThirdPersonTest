using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    // This script implements the IInteractable interface for a chest object.
    // It allows interaction with the chest, such as opening or closing it.

    [SerializeField] private Animator animator;
   

    public void Interact()
    {
        // Implement the interaction logic here
        Debug.Log("Interacted with chest.");
        ToggleChest();
    }

    private void ToggleChest()
    {
        animator.SetTrigger("Open");
        // Logic to toggle the chest's state (open/close)
        // You can add animations or sound effects here
    }
}
