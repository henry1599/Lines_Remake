using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private Cell startNode;
    private Cell endNode;
    private Grid grid;
    private List<Cell> openList;
    private List<Cell> closedList;
    public event Action<bool> OnPathFound;
    public void Init(Grid _grid, Cell _startNode, Cell _endNode)
    {
        startNode = _startNode;
        endNode = _endNode;
        grid = _grid;
    }
    public List<Cell> FindPath(BallType ballType)
    {
        print("Start finding path...");
        openList = new List<Cell> {startNode};
        closedList = new List<Cell>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Cell pathCell = grid.GetGridArray()[x, y];
                pathCell.GCost = int.MaxValue;
                pathCell.CalculateFCost();
                pathCell.FromCell = null;
            }
        }
        startNode.GCost = 0;
        startNode.HCost = GetDistance(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            Cell currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // * End here -> We find the path !!!
                print("Found path !!!");
                OnPathFound?.Invoke(true);
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Cell> neighbourNodes = new List<Cell>();
            switch (ballType)
            {
                case BallType.Normal:
                case BallType.Bomb:
                    neighbourNodes = GetNeighborNodesNormal(currentNode);
                    break;
                case BallType.Ghost:
                    neighbourNodes = GetNeighborNodesGhost(currentNode);
                    break;
                case BallType.Diagonal:
                    neighbourNodes = GetNeighborNodesDiagonal(currentNode);
                    break;
            }
            foreach (Cell neighborCell in neighbourNodes)
            {
                if (closedList.Contains(neighborCell))
                {
                    continue;
                }
                int tempGCost = currentNode.GCost + GetDistance(currentNode, neighborCell);
                if(tempGCost < neighborCell.GCost)
                {
                    neighborCell.FromCell = currentNode;
                    neighborCell.GCost = tempGCost;
                    neighborCell.HCost = GetDistance(neighborCell, endNode);
                    neighborCell.CalculateFCost();

                    if (!openList.Contains(neighborCell))
                    {
                        openList.Add(neighborCell);
                    }
                }
            }
        }
        // * End algorithm and cannot find any path
        print("Cannot find path !!!");
        OnPathFound?.Invoke(false);
        return null;
    }
    private List<Cell> GetNeighborNodesNormal(Cell currentNode)
    {
        List<Cell> result = new List<Cell>();
        if (currentNode.GetLocalX() - 1 >= 0)
        {
            // TODO: Left-center neighbour
            Cell consideredLeftNode = GetNode(currentNode.GetLocalX() - 1, currentNode.GetLocalY());
            if (consideredLeftNode.IsWalkable() == true)
            {
                result.Add(consideredLeftNode);
            }
        }
        if (currentNode.GetLocalX() + 1 < grid.GetWidth())
        {
            // TODO: Right-center neighbour
            Cell consideredRightNode = GetNode(currentNode.GetLocalX() + 1, currentNode.GetLocalY());
            if (consideredRightNode.IsWalkable() == true)
            {
                result.Add(consideredRightNode);
            }
        }
        if (currentNode.GetLocalY() - 1 >= 0)
        {
            // TODO: Bottom-center node
            Cell considerBottomNode = GetNode(currentNode.GetLocalX(), currentNode.GetLocalY() - 1);
            if (considerBottomNode.IsWalkable() == true)
            {
                result.Add(considerBottomNode);
            }
        }
        if (currentNode.GetLocalY() + 1 < grid.GetHeight())
        {
            // TODO: Top-center node
            Cell considerTopNode = GetNode(currentNode.GetLocalX(), currentNode.GetLocalY() + 1);
            if (considerTopNode.IsWalkable() == true)
            {
                result.Add(considerTopNode);
            }
        }

        return result;
    }
    private List<Cell> GetNeighborNodesGhost(Cell currentNode)
    {
        List<Cell> result = new List<Cell>();
        if (currentNode.GetLocalX() - 1 >= 0)
        {
            // TODO: Left-center neighbour
            Cell consideredLeftNode = GetNode(currentNode.GetLocalX() - 1, currentNode.GetLocalY());
            result.Add(consideredLeftNode);
        }
        if (currentNode.GetLocalX() + 1 < grid.GetWidth())
        {
            // TODO: Right-center neighbour
            Cell consideredRightNode = GetNode(currentNode.GetLocalX() + 1, currentNode.GetLocalY());
            result.Add(consideredRightNode);
        }
        if (currentNode.GetLocalY() - 1 >= 0)
        {
            // TODO: Bottom-center node
            Cell considerBottomNode = GetNode(currentNode.GetLocalX(), currentNode.GetLocalY() - 1);
            result.Add(considerBottomNode);
        }
        if (currentNode.GetLocalY() + 1 < grid.GetHeight())
        {
            // TODO: Top-center node
            Cell considerTopNode = GetNode(currentNode.GetLocalX(), currentNode.GetLocalY() + 1);
            result.Add(considerTopNode);
        }

        return result;
    }
    private List<Cell> GetNeighborNodesDiagonal(Cell currentNode)
    {
        List<Cell> result = new List<Cell>();
        if (currentNode.GetLocalX() - 1 >= 0)
        {
            // TODO: Left-center neighbour
            Cell consideredLeftNode = GetNode(currentNode.GetLocalX() - 1, currentNode.GetLocalY());
            if (consideredLeftNode.IsWalkable() == true)
            {
                result.Add(consideredLeftNode);
            }
            // TODO: Bottom-left neighbour
            if (currentNode.GetLocalY() - 1 >= 0)
            {
                Cell consideredBottomLeftNode = GetNode(currentNode.GetLocalX() - 1, currentNode.GetLocalY() - 1);
                if (consideredBottomLeftNode.IsWalkable() == true)
                {
                    result.Add(consideredBottomLeftNode);
                }
            }
            // TODO: Top-left neighbour
            if (currentNode.GetLocalY() + 1 < grid.GetHeight())
            {
                Cell consideredTopLeftNode = GetNode(currentNode.GetLocalX() - 1, currentNode.GetLocalY() + 1);
                if (consideredTopLeftNode.IsWalkable() == true)
                {
                    result.Add(consideredTopLeftNode);
                }
            }
        }
        if (currentNode.GetLocalX() + 1 < grid.GetWidth())
        {
            // TODO: Right-center neighbour
            Cell consideredRightNode = GetNode(currentNode.GetLocalX() + 1, currentNode.GetLocalY());
            if (consideredRightNode.IsWalkable() == true)
            {
                result.Add(consideredRightNode);
            }
            // TODO: Bottom-right neighbour
            if (currentNode.GetLocalY() - 1 >= 0)
            {
                Cell consideredBottomRightNode = GetNode(currentNode.GetLocalX() + 1, currentNode.GetLocalY() - 1);
                if (consideredBottomRightNode.IsWalkable() == true)
                {
                    result.Add(consideredBottomRightNode);
                }
            }
            // TODO: Top-right neighbour
            if (currentNode.GetLocalY() + 1 < grid.GetHeight())
            {
                Cell consideredTopRightNode = GetNode(currentNode.GetLocalX() + 1, currentNode.GetLocalY() + 1);
                if (consideredTopRightNode.IsWalkable() == true)
                {
                    result.Add(consideredTopRightNode);
                }
            }
        }
        if (currentNode.GetLocalY() - 1 >= 0)
        {
            // TODO: Bottom-center node
            Cell considerBottomNode = GetNode(currentNode.GetLocalX(), currentNode.GetLocalY() - 1);
            if (considerBottomNode.IsWalkable() == true)
            {
                result.Add(considerBottomNode);
            }
        }
        if (currentNode.GetLocalY() + 1 < grid.GetHeight())
        {
            // TODO: Top-center node
            Cell considerTopNode = GetNode(currentNode.GetLocalX(), currentNode.GetLocalY() + 1);
            if (considerTopNode.IsWalkable() == true)
            {
                result.Add(considerTopNode);
            }
        }

        return result;
    }
    private Cell GetNode(int x, int y)
    {
        return grid.GetGridArray()[x, y];
    }
    public int GetDistance(Cell a, Cell b)
    {
        int xDistance = Mathf.Abs(a.GetLocalX() - b.GetLocalX());
        int yDistance = Mathf.Abs(a.GetLocalY() - b.GetLocalY());
        int remainingDistance = Mathf.Abs(xDistance - yDistance);
        int result = MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remainingDistance;
        return result;
    }
    private Cell GetLowestFCostNode(List<Cell> cellList)
    {
        Cell lowestFCostNode = cellList[0];
        for (int i = 1; i < cellList.Count; i++)
        {
            lowestFCostNode = cellList[i].FCost < lowestFCostNode.FCost ? cellList[i] : lowestFCostNode;
        }
        return lowestFCostNode;
    }
    private List<Cell> CalculatePath(Cell cell)
    {
        List<Cell> result = new List<Cell>();
        result.Add(cell);
        Cell currentNode = cell;
        while (currentNode.FromCell != null)
        {
            result.Add(currentNode.FromCell);
            currentNode = currentNode.FromCell;
        }
        result.Reverse();
        return result;
    }
}
