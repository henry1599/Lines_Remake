using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector2 gridStartPosition;
    private Cell[,] gridArray;
    public Cell[,] GetGridArray() => gridArray;
    public int GetWidth() => width;
    public int GetHeight() => height;
    public float GetCellSize() => cellSize;
    public Vector2 GetStartGridPosition() => gridStartPosition;

    /// <summary>
    /// * Constructor without startPosition
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellSize"></param>
    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridStartPosition = Vector3.zero;

        gridArray = new Cell[width, height];
    }
    /// <summary>
    /// * Constructor with startPosition
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellSize"></param>
    /// <param name="startPosition"></param>
    public Grid(int width, int height, float cellSize, Vector3 startPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridStartPosition = startPosition;

        gridArray = new Cell[width, height];
    }
    public Vector2 GetWorldPosition(float x, float y)
    {
        return (new Vector2(x * cellSize, y * cellSize)) + gridStartPosition;
    }
}
