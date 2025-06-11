using System.Collections;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour, IMessage, IScoreManager
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float hintMessageDelay = 3f; // Delay before hiding the hint message
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TMP_Text scoreText;
    public int TotalScore { get; set; }
    void Start()
    {
        StartCoroutine(HintMessage(hintMessageDelay));
    }
    public void HideMessage()
    {
        messagePanel.SetActive(false);
        messageText.text = string.Empty;
        // Debug.Log("Message hidden.");
    }
    public void ShowMessage(string message)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
        // Debug.Log("Message displayed: " + message);
    }
    
    IEnumerator HintMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        hintPanel.SetActive(false);
    }

    public void AddScore(int score)
    {
        TotalScore += score;
        scoreText.text = "Score: " + TotalScore.ToString();
    }

    public int GetScore()
    {
        return TotalScore;
    }
}
