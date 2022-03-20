using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplayManager : MonoBehaviour
{
    public static GameDisplayManager Instance {get; set;}
    [SerializeField] private GameOverDisplay gameOverPanel;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TimeManager timeManager;
    void Awake() 
    {
        Instance = this;
        GameManager.OnGameStateUpdated += HandleGameStateUpdated;
    }
    void OnDestroy()
    {
        GameManager.OnGameStateUpdated -= HandleGameStateUpdated;
    }
    void HandleGameStateUpdated(GameState gameState, PlayerData playerData)
    {
        switch (gameState)
        {
            case GameState.StartGame:
                break;
            case GameState.InGame:
                break;
            case GameState.GameOver:  
                gameOverPanel.Setup(scoreManager.Score, playerData.highScore, timeManager.Timer);
                break;
        }   
        gameOverPanel.Show(gameState == GameState.GameOver);
    }
}
