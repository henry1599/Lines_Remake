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
    [SerializeField] private Collider2D col2D;
    private Ball ball = null;
    [SerializeField] private bool isFull = false;
    private int gCost;
    private int hCost;
    private int fCost;
    private Cell fromCell;
    public int GetLocalX() => localX;
    public int GetLocalY() => localY;
    public Transform GetBallSlot() => ballSlot;
    public Ball GetBall() => ball;
    public bool IsFull 
    {
        get {return isFull;}
        set 
        {
            isFull = value;
        }
    }
    void Update()
    {
        if (IsFull)
        {
            if (ball.GetState() != BallState.Queued)
            {
                col2D.enabled = false;
            }
            else
            {
                col2D.enabled = true;
            }
        }
        else
        {
            col2D.enabled = true;
        }
    }
    public int GCost
    {
        get {return gCost;}
        set {gCost = value;}
    }
    public int HCost
    {
        get {return hCost;}
        set {hCost = value;}
    }
    public int FCost
    {
        get {return fCost;}
        set {fCost = value;}
    }
    public Cell FromCell 
    {
        get {return fromCell;}
        set {fromCell = value;}
    }
    public static event Action<Cell> OnSelected;
    public override string ToString()
    {
        return $"({localX}, {localY})";
    }
    public bool IsAbleToSpawnHere()
    {
        return IsFull == false;
    }
    public bool IsContainReadyBall(out Ball _ball)
    {
        if (IsFull == false)
        {
            _ball = null;
            return false;
        }
        if (ball.GetState() == BallState.Queued)
        {
            _ball = null;
            return false;
        }
        _ball = ball;
        return true;
    }
    public bool IsContainQueuedBall(out Ball _ball)
    {
        if (IsFull == false)
        {
            _ball = null;
            return false;
        }
        if (ball.GetState() != BallState.Queued)
        {
            _ball = null;
            return false;
        }
        _ball = ball;
        return true;
    }
    public bool IsWalkable()
    {
        if (IsFull == false)
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
        IsFull = true;
        if (ball.GetState() == BallState.Queued)
        {
            col2D.enabled = true;
        }
        else
        {
            col2D.enabled = false;
        }
    }
    public void SetNewBallSlot(Ball _ball)
    {
        Helper.DeletChildren(ballSlot);
        ball = _ball;
        IsFull = true;
        if (ball.GetState() == BallState.Queued)
        {
            col2D.enabled = true;
        }
        else
        {
            col2D.enabled = false;
        }
    }
    public void UnSetBallSlot()
    {
        ball = null;
        IsFull = false;
        col2D.enabled = true;
    }
    public void RemoveBall()
    {
        IsFull = false;
        col2D.enabled = true;
        ball.UpdateState(BallState.Destroy);
        ball = null;
        // Helper.DeletChildren(ballSlot);
    }
    void OnMouseDown()
    {
        if (GameManager.Instance.GetGameState() == GameState.GameOver)
        {
            return;
        }
        if (GameManager.Instance.GetGameState() == GameState.BallMoving)
        {
            return;
        }
        if (GameManager.Instance.GetGameState() == GameState.Pausing)
        {
            return;
        }
        // print("Cell Click !");
        OnSelected?.Invoke(this);
    }
    public Vector2 GetWorldPosition()
    {
        return (new Vector2(localX * GridManager.Instance.GetGrid().GetCellSize(), localY * GridManager.Instance.GetGrid().GetCellSize())) + GridManager.Instance.GetGrid().GetStartGridPosition();
    }
    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }
}
