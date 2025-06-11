using UnityEngine;

public class InteractableLight : MonoBehaviour, IInteractable
{
    // This script implements the IInteractable interface for a light object.
    // It allows interaction with the light, such as turning it on or off.

    [SerializeField] private Light lightSource;

    public void Interact()
    {
        // Implement the interaction logic here
        Debug.Log("Interacted with light.");
        ToggleLight();
    }

    private void ToggleLight()
    {
        if (lightSource != null)
        {
            lightSource.enabled = !lightSource.enabled;
            Debug.Log("Light toggled: " + (lightSource.enabled ? "On" : "Off"));
        }
        else
        {
            Debug.LogWarning("Light source is not assigned.");
        }
    }
}
