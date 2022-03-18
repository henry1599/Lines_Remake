using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    private Vector3 desiredPosition;
    private float desiredSize;
    private Camera mainCamera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float scaleSpeed;
    private bool isLock = true;
    // Start is called before the first frame update
    void Start()
    {
        GridManager.OnFinishInitGrid += HandleFinishInitGrid;
    }
    void OnDestroy()
    {
        GridManager.OnFinishInitGrid -= HandleFinishInitGrid;    
    }
    void Update()
    {
        if (isLock)
        {
            return;
        }
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, moveSpeed * Time.deltaTime);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, desiredSize, scaleSpeed * Time.deltaTime);
    }
    void HandleFinishInitGrid()
    {
        float gridHeight = GridManager.Instance.GetGrid().GetHeight();
        float gridWidth = GridManager.Instance.GetGrid().GetWidth();
        float cellSize = GridManager.Instance.GetGrid().GetCellSize();

        float desiredX = gridWidth % 2 == 0 ? (int)((gridWidth * cellSize) / 2) - 0.5f : (int)((gridWidth * cellSize) / 2);
        float desiredY = gridHeight % 2 == 0 ? (int)((gridWidth * cellSize) / 2) - 0.5f : (int)((gridHeight * cellSize) / 2);
        
        desiredPosition = new Vector3(desiredX, desiredY, -10);
        desiredSize = Mathf.Max(gridHeight, gridWidth) / 2 + 1;
        mainCamera = Helper.Camera;

        isLock = false;
    }
}
