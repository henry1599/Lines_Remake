using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplayManager : MonoBehaviour
{
    public static GameDisplayManager Instance {get; set;}
    [SerializeField] private GameObject gameOverPanel;
    void Awake() 
    {
        Instance = this;
        GameManager.OnGameStateUpdated += HandleGameStateUpdated;
    }
    void OnDestroy()
    {
        GameManager.OnGameStateUpdated -= HandleGameStateUpdated;
    }
    void HandleGameStateUpdated(GameState gameState)
    {
        gameOverPanel.SetActive(gameState == GameState.GameOver);
        switch (gameState)
        {
            case GameState.StartGame:
                break;
            case GameState.InGame:
                break;
            case GameState.GameOver:
                break;
        }   
    }
}
