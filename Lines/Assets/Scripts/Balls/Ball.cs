using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Ball : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BallState ballState;
    [SerializeField] private Collider2D col2D;
    private int localX;
    private int localY;
    private Cell destinationCell = null;
    private Cell currentCell = null;
    public int LocalX() => localX;
    public int LocalY() => localY;
    public BallState GetState() => ballState;
    public void SetDestination(Cell newValue) => destinationCell = newValue;
    public Cell GetCurrentCell() => currentCell;
    public static event Action<Ball> OnSelected;
    public void Init(Color color, int x, int y, Cell _currentCell)
    {
        spriteRenderer.color = color;
        localX = x;
        localY = y;
        ballState = BallState.Queued;
        currentCell = _currentCell;
    }
    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.GameOver)
        {
            return;
        }
        switch (ballState)
        {
            case BallState.Queued:
                HandleQueued();
                break;
            case BallState.Ready:
                HandleReady();
                break;
            case BallState.Moving:
                HandleMoving();
                break;
            case BallState.Selected:
                HandleSelected();
                break;
            case BallState.Stop:
                HandleStop();
                break;
            case BallState.Destroy:
                HandleDestroy();
                break;
        }
    }
    void HandleQueued()
    {
        col2D.enabled = false;
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    void HandleReady()
    {
        col2D.enabled = true;
        transform.localScale = new Vector3(1, 1, 1);
    }
    void HandleMoving()
    {
        if (destinationCell == null)
        {
            return;
        }
        MoveTo(destinationCell.transform.position);
    }
    void HandleSelected()
    {

    }
    void HandleStop()
    {
        GridManager.Instance.UpdateBall();
        transform.SetParent(destinationCell.transform);

        currentCell.UnSetBallSlot();
        destinationCell.SetNewBallSlot(this);

        GameManager.Instance.GetSpawnManager().Spawn();
        UpdateState(BallState.Ready);
    }
    void HandleDestroy()
    {
        
    }
    void MoveTo(Vector2 target)
    {
        transform.position = target;
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            UpdateState(BallState.Stop);
        }
    }
    public void UpdateState(BallState newBallState)
    {
        if (ballState == newBallState)
        {
            return;
        }
        ballState = newBallState;
    }

    public void OnMouseDown()
    {
        if (GameManager.Instance.GetGameState() == GameState.GameOver)
        {
            return;
        }
        print("Ball Click !");
        OnSelected?.Invoke(this);
        if (ballState != BallState.Queued)
        {
            UpdateState(BallState.Selected);
        }
    }
}
