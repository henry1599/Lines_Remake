using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Color[] ballColors = new Color[7];
    [SerializeField] private Ball ballPrefab = null;
    private bool isAllowedToSpawn = false;
    void Start()
    {
        GridManager.OnEnoughSpaceToSpawn += HandleSpaceToSpawn;
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
        foreach (Cell emptyCell in emptyCells)
        {
            // TODO: Get a random color
            Color randomColor = ballColors[UnityEngine.Random.Range(0, ballColors.Length)];

            // TODO: Clear the cell
            Helper.DeletChildren(emptyCell.GetBallSlot());

            // TODO: Instantiate a ball instance
            Ball ballInstance = Instantiate(ballPrefab, emptyCell.GetBallSlot());

            // TODO: Init a ball
            ballInstance.Init(randomColor, emptyCell.GetLocalX(), emptyCell.GetLocalY(), emptyCell);

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
    }
    private void HandleSpaceToSpawn(bool isEnoughSpace) => isAllowedToSpawn = isEnoughSpace;
}
