using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    [SerializeField] private SpawnManager spawnManager = null;
    [SerializeField] private GameState gameState;
    [SerializeField] private int numberOfMatchedBalls = 5;
    [SerializeField] private int numberOfColor;
    private Ball selectedBall = null;
    private Cell selectedCell = null;
    public GameState GetGameState() => gameState;
    public SpawnManager GetSpawnManager() => spawnManager;
    public int GetNumberOfMatchBalls() => numberOfMatchedBalls;
    public int GetNumberOfColor() => numberOfColor;
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
        // TODO: Load highscore from computer
        ScoreManager.Instance.Score = 0;
        ScoreManager.Instance.HighScore = 0;

        Ball.OnSelected += HandleBallSelected;
        Ball.OnStopMoving += HandleBallStopMoving;
        Cell.OnSelected += HandleCellSelected;
        UpdateState(GameState.StartGame);
    }
    void OnDestroy()
    {
        Ball.OnSelected -= HandleBallSelected;
        Ball.OnStopMoving -= HandleBallStopMoving;
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
            case GameState.BallMoving:
                HandleBallMoving();
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
    void HandleBallMoving()
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
        if (selectedBall != null)
        {
            selectedBall.UpdateState(BallState.Ready);
        }
        selectedBall = ball;
    }
    void HandleCellSelected(Cell cell)
    {
        if (selectedBall == null)
        {
            return;
        }
        selectedCell = cell;

        selectedBall.DestinationCell = selectedCell;
        selectedBall.UpdateState(BallState.Moving);
        selectedBall = null;
    }
    void HandleBallStopMoving(Ball ball)
    {
        List<Cell> matchedCells = GridManager.Instance.Check(ball);
        foreach (Cell matchedCell in matchedCells)
        {
            matchedCell.RemoveBall();
        }
        GridManager.Instance.UpdateBall();
        UpdateState(GameState.InGame);
        spawnManager.Spawn();
    }
}