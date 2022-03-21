using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    [SerializeField] private SpawnManager spawnManager = null;
    [SerializeField] private ScoreManager scoreManager = null;
    [SerializeField] private TimeManager timeManager = null;
    [SerializeField] private GameState gameState;
    private int numberOfMatchedBalls;
    private bool isMute = false;
    public bool IsMute
    {
        get {return isMute;}
        set {isMute = value;}
    }
    private int numberOfColor;
    private PlayerData playerData;
    private Ball selectedBall = null;
    private Cell selectedCell = null;
    public GameState GetGameState() => gameState;
    public SpawnManager GetSpawnManager() => spawnManager;
    public int GetNumberOfMatchBalls() => numberOfMatchedBalls;
    public int GetNumberOfColor() => numberOfColor;
    public static event Action<GameState, PlayerData> OnGameStateUpdated;
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
        numberOfMatchedBalls = SettingManager.Instance.ChosenLevel.ballsToCollect;
        numberOfColor = SettingManager.Instance.ChosenLevel.colorAmount;
        // TODO: Load highscore from computer
        playerData = SaveSystem.LoadPlayer();
        ScoreManager.Instance.Score = 0;
        ScoreManager.Instance.HighScore = playerData.highScore;

        Ball.OnSelected += HandleBallSelected;
        Ball.OnStopMoving += HandleBallStopMoving;
        Cell.OnSelected += HandleCellSelected;
        Buttons.OnSkipTurnButtonClick += HandleSkipTurn;
        UpdateState(GameState.StartGame);
    }
    void OnDestroy()
    {
        Ball.OnSelected -= HandleBallSelected;
        Ball.OnStopMoving -= HandleBallStopMoving;
        Cell.OnSelected -= HandleCellSelected;
        Buttons.OnSkipTurnButtonClick -= HandleSkipTurn;
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
            case GameState.Pausing:
                HandlePausing();
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
        SaveSystem.SavePlayer(scoreManager, timeManager);
    }
    void HandlePausing()
    {

    }
    public void UpdateState(GameState newGameState)
    {
        if (newGameState == gameState)
        {
            return;
        }
        gameState = newGameState;
        OnGameStateUpdated?.Invoke(gameState, playerData);
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
    void HandleSkipTurn()
    {
        GridManager.Instance.UpdateBall();
        UpdateState(GameState.InGame);
        spawnManager.Spawn();
        List<Cell> emptyRemaningCells = GridManager.Instance.GetAllSpawnableCells();
        // print(emptyRemaningCells.Count);
        if (emptyRemaningCells.Count == 0)
        {
            GameManager.Instance.UpdateState(GameState.GameOver);
        }
    }
}