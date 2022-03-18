using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform ballSlot;
    [SerializeField] private int localX;
    [SerializeField] private int localY;
    private Ball ball = null;
    [SerializeField] private bool isFull = false;
    public int GetLocalX() => localX;
    public int GetLocalY() => localY;
    public Transform GetBallSlot() => ballSlot;
    public Ball GetBall() => ball;
    public static event Action<Cell> OnSelected;
    public bool IsAbleToSpawnHere()
    {
        return isFull == false;
    }
    public bool IsContainsBall()
    {
        return isFull == true;
    }
    public bool IsWalkthrough()
    {
        if (isFull == false)
        {
            return true;
        }
        else
        {
            if (ball.GetState() != BallState.Queued)
            {
                return false;
            }
            return true;
        }
    }

    public void SetLocalPosition(int x, int y)
    {
        localX = x;
        localY = y;
    }
    public void SetBallSlot(Ball _ball)
    {
        ball = _ball;
        isFull = true;
        GridManager.Instance.NumberOfEmptyCell--;
    }
    public void SetNewBallSlot(Ball _ball)
    {
        Helper.DeletChildren(ballSlot);
        ball = _ball;
        isFull = true;
        GridManager.Instance.NumberOfEmptyCell--;
    }
    public void UnSetBallSlot()
    {
        ball = null;
        isFull = false;
        GridManager.Instance.NumberOfEmptyCell++;
    }
    void OnMouseDown()
    {
        if (GameManager.Instance.GetGameState() == GameState.GameOver)
        {
            return;
        }
        print("Cell Click !");
        OnSelected?.Invoke(this);
        
    }
    public Vector2 GetWorldPosition()
    {
        return (new Vector2(localX * GridManager.Instance.GetGrid().GetCellSize(), localY * GridManager.Instance.GetGrid().GetCellSize())) + GridManager.Instance.GetGrid().GetStartGridPosition();
    }
}
