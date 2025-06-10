using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour, IMessage
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;

    public void HideMessage()
    {
        messagePanel.SetActive(false);
        messageText.text = string.Empty;
        Debug.Log("Message hidden.");
    }
    public void ShowMessage(string message)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
        Debug.Log("Message displayed: " + message);
    }
}
