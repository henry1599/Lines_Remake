using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 startGridPosition;
    [SerializeField] private Cell cellObject;
    private Grid grid;
    public Grid GetGrid() => grid;
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
    }
}
