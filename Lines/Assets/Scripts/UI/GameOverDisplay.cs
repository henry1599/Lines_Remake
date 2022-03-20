using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TimerDisplay timerDisplay;
    public void Setup(int score, int highScore, float timer)
    {
        scoreText.text = score.ToString();
        highscoreText.text = highScore.ToString();
        timeText.text = timerDisplay.GetTimeString(timer);
    }
    public void Show(bool status)
    {
        gameOverPanel.SetActive(status);
    }
}
