using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    // This script implements the IInteractable interface for a chest object.
    // It allows interaction with the chest, such as opening or closing it.

    [SerializeField] private Animator animator;
    [SerializeField] private Material glowMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject chestLid;
    public void Interact()
    {
        // Implement the interaction logic here
        Debug.Log("Interacted with chest.");
        ToggleChest();
    }

    private void ToggleChest()
    {
        animator.SetTrigger("Open");
        StartCoroutine(ChangeMaterial());
        // Logic to toggle the chest's state (open/close)
        // You can add animations or sound effects here
    }
    IEnumerator ChangeMaterial()
    {

        if (glowMaterial != null && defaultMaterial != null)
        {
            chest.GetComponent<Renderer>().material = glowMaterial;
            chestLid.GetComponent<Renderer>().material = glowMaterial;
            yield return new WaitForSeconds(1.8f);
            chest.GetComponent<Renderer>().material = defaultMaterial;
            chestLid.GetComponent<Renderer>().material = defaultMaterial;
        }
        else
        {
            Debug.LogWarning("Glow or default material not assigned.");
        }
    }
}
