using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance {get; set;}
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 startGridPosition;
    [SerializeField] private Cell cellObject;
    private int numberOfEmptyCell = 0;
    private Grid grid;
    public Grid GetGrid() => grid;
    public int NumberOfEmptyCell
    {
        get { return numberOfEmptyCell;}
        set
        {
            numberOfEmptyCell = value;
        }
    }
    public static event Action OnFinishInitGrid;
    public static event Action<bool> OnEnoughSpaceToSpawn;
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
        InitGrid();
    }
    public void InitGrid()
    {
        grid = new Grid(width, height, cellSize, startGridPosition);
        for (int i = 0; i < grid.GetWidth(); i++)
        {
            for (int j = 0; j < grid.GetHeight(); j++)
            {
                grid.GetGridArray()[i, j] = Instantiate(cellObject.gameObject, grid.GetWorldPosition(i, j), Quaternion.identity).GetComponent<Cell>();
                grid.GetGridArray()[i, j].SetLocalPosition(i, j);
            }
        }
        NumberOfEmptyCell = width * height;
        OnFinishInitGrid?.Invoke();
    }
    public List<Cell> GetRandomSpawnableCells(int numberOfRandomCells)
    {
        List<Cell> emptyRemaningCells = GetAllSpawnableCells();
        if (emptyRemaningCells.Count == 1)
        {
            OnEnoughSpaceToSpawn?.Invoke(false);
            GameManager.Instance.UpdateState(GameState.GameOver);
            return new List<Cell>();
        }
        else
        {
            OnEnoughSpaceToSpawn?.Invoke(true);
        }
        numberOfRandomCells = Mathf.Min(numberOfRandomCells, emptyRemaningCells.Count);
        List<Cell> result = new List<Cell>();
        List<int> randomedIdx = new List<int>();
        for (int i = 0; i < numberOfRandomCells;)
        {
            int randomIdx = UnityEngine.Random.Range(0, emptyRemaningCells.Count);
            if (randomedIdx.Contains(randomIdx))
            {
                continue;
            }
            randomedIdx.Add(randomIdx);
            result.Add(emptyRemaningCells[randomIdx]);
            i++;
        }
        return result;
    }
    public List<Cell> GetAllSpawnableCells()
    {
        List<Cell> result = new List<Cell>();
        foreach (Cell cell in grid.GetGridArray())
        {
            if (cell.IsAbleToSpawnHere() == true)
            {
                result.Add(cell);
            }
        }
        return result;
    }
    public void UpdateBall()
    {
        List<Cell> result = new List<Cell>();
        foreach (Cell cell in grid.GetGridArray())
        {
            if (cell.IsContainsBall() == true)
            {
                Ball ball = cell.GetBall();
                if (ball.GetState() == BallState.Queued)
                {
                    ball.UpdateState(BallState.Ready);
                }
            }
        }
    }
}
