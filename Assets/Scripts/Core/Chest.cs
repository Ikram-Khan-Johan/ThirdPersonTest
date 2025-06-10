using System.Collections;
using Mono.Cecil.Cil;
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
    [SerializeField] private GameObject coinPrefab;
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
            CollectCoins();
            yield return new WaitForSeconds(1.8f);
            chest.GetComponent<Renderer>().material = defaultMaterial;
            chestLid.GetComponent<Renderer>().material = defaultMaterial;

        }
        else
        {
            Debug.LogWarning("Glow or default material not assigned.");
        }
    }

    private void CollectCoins()
    {

        for (int i = 0; i < 10; i++)
        {
            var coin = Instantiate(coinPrefab, transform.position, Quaternion.identity) as GameObject;

            coin.transform.SetParent(transform);
            coin.transform.localPosition = new Vector3(0f, 0f, 0f);
            coin.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            coin.transform.localRotation = Quaternion.identity;
            ISoundPlayer soundPlayer = coin.GetComponent<ISoundPlayer>();
            if (soundPlayer != null)
            {
                soundPlayer.PlaySound();
            }
            else
            {
                Debug.LogWarning("ISoundPlayer component is missing on the coin prefab.");
            }
            // Logic to collect coins
            // Debug.Log("Collected coin " + (i + 1));
        }
        // You can add sound effects or animations for collecting coins
    }
}
