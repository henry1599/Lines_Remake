using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    void Awake()
    {
        ScoreManager.OnScoreUpdated += HandleScoreUpdated;
    }
    void OnDestroy()
    {
        ScoreManager.OnScoreUpdated -= HandleScoreUpdated;    
    }
    void HandleScoreUpdated(int score, int highScore)
    {
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }
}
