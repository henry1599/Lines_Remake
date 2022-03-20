using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance {get; set;}
    private int width;
    private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 startGridPosition;
    [SerializeField] private Cell cellObject;
    private Grid grid;
    public Grid GetGrid() => grid;
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
        width = SettingManager.Instance.ChosenLevel.width;
        height = SettingManager.Instance.ChosenLevel.height;
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
        OnFinishInitGrid?.Invoke();
    }
    public List<Cell> GetRandomSpawnableCells(int numberOfRandomCells)
    {
        List<Cell> emptyRemaningCells = GetAllSpawnableCells();
        // print(emptyRemaningCells.Count);
        if (emptyRemaningCells.Count == 1)
        {
            // print("Game over");
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
        for (int i = 0; i < grid.GetGridArray().GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetGridArray().GetLength(1); j++)
            {
                if (grid.GetGridArray()[i, j].IsContainQueuedBall(out Ball ball))
                {
                    if (grid.GetGridArray()[i, j].GetBallSlot().childCount > 0)
                    {
                        grid.GetGridArray()[i, j].IsFull = true;
                    }
                    ball.UpdateState(BallState.Ready);
                    if (ball.GetBallType() != BallType.Bomb)
                    {
                        List<Cell> matchedCells = Check(ball);
                        foreach (Cell matchedCell in matchedCells)
                        {
                            matchedCell.RemoveBall();
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// * Check all neighbor nodes (at most 8 nodes) around the given balls
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    public List<Cell> GetNeighbourAround(Ball ball)
    {
        List<Cell> result = new List<Cell>();
        // TODO: Center-left
        if (ball.GetCurrentCell().GetLocalX() - 1 >= 0)
        {
            Cell considerCenterLeftCell = grid.GetGridArray()[ball.GetCurrentCell().GetLocalX() - 1, ball.GetCurrentCell().GetLocalY()];
            if (considerCenterLeftCell.IsContainReadyBall(out Ball _ballCenterLeft))
            {
                result.Add(considerCenterLeftCell);
            }
            // TODO: Bottom-left
            if (ball.GetCurrentCell().GetLocalY() - 1 >= 0)
            {
                Cell consideredBottomLeftCell = grid.GetGridArray()[ball.GetCurrentCell().GetLocalX() - 1, ball.GetCurrentCell().GetLocalY() - 1];
                if (consideredBottomLeftCell.IsContainReadyBall(out Ball _ballBottomLeft))
                {
                    result.Add(consideredBottomLeftCell);
                }
            }
            // TODO: Top-left
            if (ball.GetCurrentCell().GetLocalY() + 1 < grid.GetHeight())
            {
                Cell consideredTopLeftCell = grid.GetGridArray()[ball.GetCurrentCell().GetLocalX() - 1, ball.GetCurrentCell().GetLocalY() + 1];
                if (consideredTopLeftCell.IsContainReadyBall(out Ball _ballTopLeft))
                {
                    result.Add(consideredTopLeftCell);
                }
            }
        }
        // TODO: Center-right
        if (ball.GetCurrentCell().GetLocalX() + 1 < grid.GetWidth())
        {
            Cell consideredCenterRightCell = grid.GetGridArray()[ball.GetCurrentCell().GetLocalX() + 1, ball.GetCurrentCell().GetLocalY()];
            if (consideredCenterRightCell.IsContainReadyBall(out Ball _ballCenterRight))
            {
                result.Add(consideredCenterRightCell);
            }
            // TODO: Bottom-right
            if (ball.GetCurrentCell().GetLocalY() - 1 >= 0)
            {
                Cell consideredBottomRightCell = grid.GetGridArray()[ball.GetCurrentCell().GetLocalX() + 1, ball.GetCurrentCell().GetLocalY() - 1];
                if (consideredBottomRightCell.IsContainReadyBall(out Ball _ballBottomRight))
                {
                    result.Add(consideredBottomRightCell);
                }
            }
            // TODO: Top-right
            if (ball.GetCurrentCell().GetLocalY() + 1 < grid.GetHeight())
            {
                Cell consideredTopRightCell = grid.GetGridArray()[ball.GetCurrentCell().GetLocalX() + 1, ball.GetCurrentCell().GetLocalY() + 1];
                if (consideredTopRightCell.IsContainReadyBall(out Ball _ballTopRight))
                {
                    result.Add(consideredTopRightCell);
                }
            }
        }
        // TODO: Bottm-center
        if (ball.GetCurrentCell().GetLocalY() - 1 >= 0)
        {
            Cell consideredBottomCenterCell = grid.GetGridArray()[ball.GetCurrentCell().GetLocalX(), ball.GetCurrentCell().GetLocalY() - 1];
            if (consideredBottomCenterCell.IsContainReadyBall(out Ball _ballBottomCenter))
            {
                result.Add(consideredBottomCenterCell);
            }
        }
        // TODO: Top-center
        if (ball.GetCurrentCell().GetLocalY() + 1 < grid.GetHeight())
        {
            Cell consideredTopCenterCell = grid.GetGridArray()[ball.GetCurrentCell().GetLocalX(), ball.GetCurrentCell().GetLocalY() + 1];
            if (consideredTopCenterCell.IsContainReadyBall(out Ball _ballTopCenter))
            {
                result.Add(consideredTopCenterCell);
            }
        }
        return result;
    }
    /// <summary>
    /// * Check all directions (Horizontal, Vertical and Diagonal) from the given ball
    /// * return a list of cell which is match 5 or more balls in any direction
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    public List<Cell> Check(Ball ball)
    {
        if (ball.GetBallType() == BallType.Bomb)
        {
            List<Cell> resultBomb = GetNeighbourAround(ball);
            return resultBomb;
        }
        List<Cell> result = new List<Cell>();
        Cell currentCell = ball.GetCurrentCell();

        result.AddRange(CheckHorizontal(ball));
        result.AddRange(CheckVertical(ball));
        result.AddRange(CheckDiagonal1(ball));
        result.AddRange(CheckDiagonal2(ball));
        
        if (result.Count == 0)
        {
            return result;
        }
        result.Add(currentCell);
        return result;
    }
    /// <summary>
    /// * Check horizontal
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    public List<Cell> CheckHorizontal(Ball ball)
    {
        List<Cell> result = new List<Cell>();

        Cell currentCell = ball.GetCurrentCell();
        Color ballColor = ball.GetBallColor();
        int matchedBalls = 0;

        // TODO: Go left
        for (int i = currentCell.GetLocalX() - 1; i >= 0; i--)
        {
            if (!grid.GetGridArray()[i, currentCell.GetLocalY()].IsContainReadyBall(out Ball _ball))
            {
                break;
            }
            if (_ball.GetBallColor() != ballColor)
            {
                break;
            }
            if (_ball.GetBallType() != ball.GetBallType())
            {
                break;
            }
            result.Add(grid.GetGridArray()[i, currentCell.GetLocalY()]);
            matchedBalls++;
        }

        // TODO: Go right
        for (int i = currentCell.GetLocalX() + 1; i < width; i++)
        {
            if (!grid.GetGridArray()[i, currentCell.GetLocalY()].IsContainReadyBall(out Ball _ball))
            {
                break;
            }
            if (_ball.GetBallColor() != ballColor)
            {
                break;
            }
            if (_ball.GetBallType() != ball.GetBallType())
            {
                break;
            }
            result.Add(grid.GetGridArray()[i, currentCell.GetLocalY()]);
            matchedBalls++;
        }
        
        // ! Why +1
        // ! 'Cause in 2 for loop above, we count an addition ball (the ball from the param)
        if (matchedBalls >= GameManager.Instance.GetNumberOfMatchBalls() - 1)
        {
            // print("Match horizontal");
            // result.Add(currentCell);
            return result;
        }
        else
        {
            // print("Dont match horizontal");
            return new List<Cell>();
        }
    }
    /// <summary>
    /// * Check vertical
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    public List<Cell> CheckVertical(Ball ball)
    {
        List<Cell> result = new List<Cell>();

        Cell currentCell = ball.GetCurrentCell();
        Color ballColor = ball.GetBallColor();
        int matchedBalls = 0;

        // TODO: Go left
        for (int i = currentCell.GetLocalY() - 1; i >= 0; i--)
        {
            if (!grid.GetGridArray()[currentCell.GetLocalX(), i].IsContainReadyBall(out Ball _ball))
            {
                break;
            }
            if (_ball.GetBallColor() != ballColor)
            {
                break;
            }
            if (_ball.GetBallType() != ball.GetBallType())
            {
                break;
            }
            result.Add(grid.GetGridArray()[currentCell.GetLocalX(), i]);
            matchedBalls++;
        }

        // TODO: Go right
        for (int i = currentCell.GetLocalY() + 1; i < height; i++)
        {
            if (!grid.GetGridArray()[currentCell.GetLocalX(), i].IsContainReadyBall(out Ball _ball))
            {
                break;
            }
            if (_ball.GetBallColor() != ballColor)
            {
                break;
            }
            if (_ball.GetBallType() != ball.GetBallType())
            {
                break;
            }
            result.Add(grid.GetGridArray()[currentCell.GetLocalX(), i]);
            matchedBalls++;
        }
        
        // ! Why +1
        // ! 'Cause in 2 for loop above, we count an addition ball (the ball from the param)
        if (matchedBalls >= GameManager.Instance.GetNumberOfMatchBalls() - 1)
        {
            // print("Match vertical");
            // result.Add(currentCell);
            return result;
        }
        else
        {
            // print("Dont match vertical");
            return new List<Cell>();
        }
    }
    /// <summary>
    /// * Check diagonal
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    public List<Cell> CheckDiagonal1(Ball ball)
    {
        List<Cell> result = new List<Cell>();

        Cell currentCell = ball.GetCurrentCell();
        Color ballColor = ball.GetBallColor();
        int matchedBalls = 0;

        // TODO: Go from bottom-left to top-right
        for (int i = currentCell.GetLocalX() - 1, j = currentCell.GetLocalY() - 1; i >= 0 && j >= 0; i--, j--)
        {
            if (!grid.GetGridArray()[i, j].IsContainReadyBall(out Ball _ball))
            {
                break;
            }
            if (_ball.GetBallColor() != ballColor)
            {
                break;
            }
            if (_ball.GetBallType() != ball.GetBallType())
            {
                break;
            }
            result.Add(grid.GetGridArray()[i, j]);
            matchedBalls++;
        }
        for (int i = currentCell.GetLocalX() + 1, j = currentCell.GetLocalY() + 1; i < width && j < height; i++, j++)
        {
            if (!grid.GetGridArray()[i, j].IsContainReadyBall(out Ball _ball))
            {
                break;
            }
            if (_ball.GetBallColor() != ballColor)
            {
                break;
            }
            if (_ball.GetBallType() != ball.GetBallType())
            {
                break;
            }
            result.Add(grid.GetGridArray()[i, j]);
            matchedBalls++;
        }
        // ! Why +3
        // ! 'Cause in 4 for loop above, we count an addition ball (the ball from the param)
        if (matchedBalls >= GameManager.Instance.GetNumberOfMatchBalls() - 1)
        {
            // print("Match diagonal");
            // result.Add(currentCell);
            return result;
        }
        else
        {
            // print("Dont match diagonal");
            return new List<Cell>();
        }
    }
    public List<Cell> CheckDiagonal2(Ball ball)
    {
        List<Cell> result = new List<Cell>();

        Cell currentCell = ball.GetCurrentCell();
        Color ballColor = ball.GetBallColor();
        int matchedBalls = 0;
        // TODO: Go from top-left to bottom-right
        for (int i = currentCell.GetLocalX() - 1, j = currentCell.GetLocalY() + 1; i >= 0 && j < height; i--, j++)
        {
            if (!grid.GetGridArray()[i, j].IsContainReadyBall(out Ball _ball))
            {
                break;
            }
            if (_ball.GetBallColor() != ballColor)
            {
                break;
            }
            if (_ball.GetBallType() != ball.GetBallType())
            {
                break;
            }
            result.Add(grid.GetGridArray()[i, j]);
            matchedBalls++;
        }
        for (int i = currentCell.GetLocalX() + 1, j = currentCell.GetLocalY() - 1; i < width && j >= 0; i++, j--)
        {
            if (!grid.GetGridArray()[i, j].IsContainReadyBall(out Ball _ball))
            {
                break;
            }
            if (_ball.GetBallColor() != ballColor)
            {
                break;
            }
            if (_ball.GetBallType() != ball.GetBallType())
            {
                break;
            }
            result.Add(grid.GetGridArray()[i, j]);
            matchedBalls++;
        }
        
        // ! Why +3
        // ! 'Cause in 4 for loop above, we count an addition ball (the ball from the param)
        if (matchedBalls >= GameManager.Instance.GetNumberOfMatchBalls() - 1)
        {
            // print("Match diagonal");
            // result.Add(currentCell);
            return result;
        }
        else
        {
            // print("Dont match diagonal");
            return new List<Cell>();
        }
    }
    public List<Ball> GetAllUpdatedBall()
    {
        List<Ball> result = new List<Ball>();
        for (int i = 0; i < grid.GetGridArray().GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetGridArray().GetLength(1); j++)
            {
                if (grid.GetGridArray()[i, j].IsContainQueuedBall(out Ball _ball) == true)
                {
                    result.Add(_ball);
                }
            }
        }
        return result;
    }
}
