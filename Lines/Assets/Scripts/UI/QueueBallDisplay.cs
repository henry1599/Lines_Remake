using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueBallDisplay : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private Transform layout;
    [SerializeField] private QueuedBall queuedBallImagePrefab = null;
    private List<QueuedBall> queuedBallImages = new List<QueuedBall>();
    void Awake()
    {
        spawnManager.OnQueuedColors += HandleQueuedColors;    
    }
    void Start()
    {
        for (int i = 0; i < SettingManager.Instance.ChosenLevel.queuedBallEachTurn; i++)
        {
            QueuedBall queuedBallImagePrefabInstance = Instantiate(queuedBallImagePrefab, layout).gameObject.GetComponent<QueuedBall>();
            queuedBallImages.Add(queuedBallImagePrefabInstance);
        }
    }
    void OnDestroy()
    {
        spawnManager.OnQueuedColors -= HandleQueuedColors;    
    }
    void HandleQueuedColors(List<Ball> balls)
    {
        // print(balls.Count);
        for (int i = 0; i < balls.Count; i ++)
        {
            queuedBallImages[i].Setup(balls[i]);
        }
    }
}
