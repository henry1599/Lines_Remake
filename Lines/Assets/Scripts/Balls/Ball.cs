using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Ball : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer graphic;
    [SerializeField] private BallState ballState;
    [SerializeField] private Collider2D col2D;
    [SerializeField] private GameObject glow;
    [SerializeField] private BallType type;
    [SerializeField] private PathFinding pathFinding;
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed;
    [SerializeField] private ParticleSystem vfxTrail;
    [SerializeField] private ParticleSystem vfxExplore;
    [SerializeField] private ParticleSystem[] vfxSelected;
    [SerializeField] private GameObject vfxCannotMove;
    private Coroutine MoveIE;
    private int localX;
    private int localY;
    private Cell destinationCell = null;
    private Cell currentCell = null;
    private List<Cell> path = null;
    private bool isMoving = false;
    private bool pathFound = false;
    public int LocalX() => localX;
    public int LocalY() => localY;
    public SpriteRenderer GetSpriteRenderer() => spriteRenderer;
    public SpriteRenderer GetGraphic() => graphic;
    public BallState GetState() => ballState;
    public BallType GetBallType() => type;
    public Cell DestinationCell
    {
        get {return destinationCell;}
        set
        {
            destinationCell = value;
            pathFinding.Init(GridManager.Instance.GetGrid(), currentCell, destinationCell);
            path = pathFinding.FindPath(type);
        }
    }
    public Cell GetCurrentCell() => currentCell;
    public Color GetBallColor() => spriteRenderer.color;
    public static event Action<Ball> OnSelected;
    public static event Action<Ball> OnStopMoving;
    public static event Action<Ball> OnHover;
    public static event Action OnExit;
    public void Init(Color color, int x, int y, Cell _currentCell)
    {
        spriteRenderer.color = color;
        if (type != BallType.Bomb)
        {
            Color glowColor = color;
            glowColor.a = 0.5f;
            glow.GetComponent<SpriteRenderer>().color = glowColor;

            ParticleSystem.MainModule mainTrailParticle = vfxTrail.main;
            ParticleSystem.MainModule mainExploreParticle = vfxExplore.main;
            ParticleSystem.MainModule mainSelectedParticle = vfxSelected[0].main;
            ParticleSystem.MainModule mainBeamParticle = vfxSelected[1].main;

            mainTrailParticle.startColor = glowColor;
            mainExploreParticle.startColor = color;
            mainSelectedParticle.startColor = color;
            mainBeamParticle.startColor = glowColor;
        }


        localX = x;
        localY = y;

        ballState = BallState.Queued;

        currentCell = _currentCell;

        pathFinding.OnPathFound += HandlePathFound;
    }
    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.GameOver)
        {
            return;
        }
        if (GameManager.Instance.GetGameState() == GameState.Pausing)
        {
            return;
        }
        anim.SetBool("selected", ballState == BallState.Selected);
        vfxSelected[0].gameObject.SetActive(ballState == BallState.Selected);
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
        spriteRenderer.sortingOrder = 1;
        if (graphic != null)
        {
            graphic.sortingOrder = 2;
        }
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    void HandleReady()
    {
        col2D.enabled = true;
        spriteRenderer.sortingOrder = 3;
        if (graphic != null)
        {
            graphic.sortingOrder = 4;
        }
        transform.localScale = new Vector3(1, 1, 1);
    }
    void HandleMoving()
    {
        spriteRenderer.sortingOrder = 30;
        if (graphic != null)
        {
            graphic.sortingOrder = 40;
        }
        if (pathFound == false)
        {
            UpdateState(BallState.Ready);
            return;
        }
        GameManager.Instance.UpdateState(GameState.BallMoving);
        MoveTo(destinationCell.transform.position);
    }
    void HandleSelected()
    {
    }
    void HandleStop()
    {
        path = null;
        isMoving = false;
        pathFound = false;


        // print($"From {currentCell.GetWorldPosition()} to {destinationCell.GetWorldPosition()}");

        currentCell.UnSetBallSlot();
        destinationCell.SetNewBallSlot(this);
        currentCell = destinationCell;

        transform.SetParent(destinationCell.transform);

        if (type == BallType.Bomb)
        {
            UpdateState(BallState.Destroy);
        }
        else
        {
            UpdateState(BallState.Ready);
        }
        OnStopMoving?.Invoke(this);
    }
    /// <summary>
    /// * Handle score here !
    /// </summary>
    void HandleDestroy()
    {
        currentCell.UnSetBallSlot();
        pathFinding.OnPathFound -= HandlePathFound;
        ParticleSystem vfxExploreInstance = Instantiate(vfxExplore.gameObject, currentCell.GetWorldPosition(), Quaternion.identity).GetComponent<ParticleSystem>();

        if (type != BallType.Bomb)
        {
            ParticleSystem.MainModule mainExploreParticle = vfxExploreInstance.main;
            mainExploreParticle.startColor = spriteRenderer.color;
            FindObjectOfType<AudioManager>().Play("Collect", false);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Explosion", false);
        }

        if (type == BallType.Bomb)
        {
            List<Cell> matchedCells = GridManager.Instance.Check(this);
            foreach (Cell matchedCell in matchedCells)
            {
                matchedCell.RemoveBall();
            }
        }

        ScoreManager.Instance.Score++;
        Destroy(gameObject);
    }
    void MoveTo(Vector2 target)
    {
        if (isMoving == false)
        {
            StartCoroutine(MoveCoroutine());
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
        if (GameManager.Instance.GetGameState() == GameState.BallMoving)
        {
            return;
        }
        if (GameManager.Instance.GetGameState() == GameState.Pausing)
        {
            return;
        }
        FindObjectOfType<AudioManager>().Play("Choose Ball", false);
        // print("Ball Click !");
        OnSelected?.Invoke(this);
        if (ballState != BallState.Queued)
        {
            UpdateState(BallState.Selected);
        }
    }
    void OnMouseEnter()
    {
        if (GameManager.Instance.GetGameState() == GameState.BallMoving)
        {
            return;
        }
        if (GameManager.Instance.GetGameState() == GameState.Pausing)
        {
            return;
        }
        OnHover?.Invoke(this);
        glow.SetActive(true);
    }
    void OnMouseExit()
    {
        OnExit?.Invoke();
        glow.SetActive(false);
    }
    IEnumerator MoveCoroutine()
    {
        isMoving = true;
        for (int i = 0; i < path.Count; i++)
        {   
            yield return StartCoroutine(Moving(i));
        }
        UpdateState(BallState.Stop);
    }

    IEnumerator Moving(int currentPosition)
    {
        while ((Vector2)transform.position != path[currentPosition].GetWorldPosition())
        {
            transform.position = Vector2.MoveTowards(transform.position, path[currentPosition].GetWorldPosition(), moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    void HandlePathFound(bool found)
    {
        pathFound = found;
    } 
}
