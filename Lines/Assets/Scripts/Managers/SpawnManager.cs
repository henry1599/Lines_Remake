using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Color> ballColors = new List<Color>();
    [SerializeField] private Ball[] ballPrefabs;
    private bool isAllowedToSpawn = false;
    public event Action<List<Ball>> OnQueuedColors;
    void Start()
    {
        GridManager.OnEnoughSpaceToSpawn += HandleSpaceToSpawn;
        ballColors = Shuffle(ballColors);
    }
    void OnDestroy()
    {
        GridManager.OnEnoughSpaceToSpawn -= HandleSpaceToSpawn;
    }
    public void Spawn()
    {
        if (GameManager.Instance.GetGameState() == GameState.GameOver)
        {
            return;
        }
        List<Cell> emptyCells = GridManager.Instance.GetRandomSpawnableCells(3);
        if (!isAllowedToSpawn)
        {
            return;
        }
        List<Ball> randomBalls = new List<Ball>();
        foreach (Cell emptyCell in emptyCells)
        {
            // TODO: Get a random color
            Color randomColor = ballColors[UnityEngine.Random.Range(0, Mathf.Min(ballColors.Count, GameManager.Instance.GetNumberOfColor()))];

            // TODO: Clear the cell
            Helper.DeletChildren(emptyCell.GetBallSlot());

            // TODO: Instantiate a ball instance
            Ball ballInstance = Instantiate(ballPrefabs[UnityEngine.Random.Range(0, ballPrefabs.Length)], emptyCell.GetBallSlot());

            // TODO: Init a ball
            ballInstance.Init(randomColor, emptyCell.GetLocalX(), emptyCell.GetLocalY(), emptyCell);
            randomBalls.Add(ballInstance);
            
            // TODO: If this function is called just after the game starts
            // TODO: make all balls to be Ready
            if (GameManager.Instance.GetGameState() == GameState.StartGame)
            {
                ballInstance.UpdateState(BallState.Ready);
            }
            
            // TODO: Set the cell slot
            emptyCell.SetBallSlot(ballInstance);
        }
        // TODO: If this function is called just after the game starts
        // TODO: change the game state
        if (GameManager.Instance.GetGameState() == GameState.StartGame)
        {
            GameManager.Instance.UpdateState(GameState.InGame);
        }
        OnQueuedColors?.Invoke(randomBalls);
    }

    private List<Color> Shuffle(List<Color> colorList)
    {
        System.Random rnd = new System.Random();
        return colorList.OrderBy(item => rnd.Next()).ToList();
    }
    private void HandleSpaceToSpawn(bool isEnoughSpace) => isAllowedToSpawn = isEnoughSpace;
}
