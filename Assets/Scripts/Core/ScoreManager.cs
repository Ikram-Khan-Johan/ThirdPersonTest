using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreManager
{
    public int TotalScore { get; set; }

    IMessage messageHandler;
    void Start()
    {
        messageHandler = FindObjectOfType<GameUIManager>();
    }
    public void AddScore(int score)
    {
        TotalScore += score;
        // Assuming there's a method to update the UI with the new score
        messageHandler.UpdateScoreText("Score: " + TotalScore);
    }

    public int GetScore()
    {
        return TotalScore;
    }

    public void UpdateScore(int score)
    {
        
    }
}
