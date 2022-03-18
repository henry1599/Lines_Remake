using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    [SerializeField] private SpawnManager spawnManager = null;
    [SerializeField] private GameState gameState;
    private Ball selectedBall = null;
    private Cell selectedCell = null;
    public GameState GetGameState() => gameState;
    public SpawnManager GetSpawnManager() => spawnManager;
    public static event Action<GameState> OnGameStateUpdated;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Ball.OnSelected += HandleBallSelected;
        Cell.OnSelected += HandleCellSelected;
        UpdateState(GameState.StartGame);
    }
    void OnDestroy()
    {
        Ball.OnSelected -= HandleBallSelected;
        Cell.OnSelected -= HandleCellSelected;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     spawnManager.Spawn();
        // }
        switch (gameState)
        {
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.InGame:
                HandleInGame();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }   
    }
    void HandleStartGame()
    {
        spawnManager.Spawn();
    }
    void HandleInGame()
    {

    }
    void HandleGameOver()
    {

    }
    public void UpdateState(GameState newGameState)
    {
        if (newGameState == gameState)
        {
            return;
        }
        gameState = newGameState;
        OnGameStateUpdated?.Invoke(gameState);
    }
    void HandleBallSelected(Ball ball)
    {
        selectedBall = ball;
    }
    void HandleCellSelected(Cell cell)
    {
        if (selectedBall == null)
        {
            return;
        }
        selectedCell = cell;

        selectedBall.SetDestination(selectedCell);
        selectedBall.UpdateState(BallState.Moving);
        selectedBall = null;
    }
}