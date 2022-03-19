using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueBallDisplay : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private List<Image> queuedBallImages = new List<Image>();
    [SerializeField] private List<Image> queuedBallGraphics = new List<Image>();
    void Awake()
    {
        spawnManager.OnQueuedColors += HandleQueuedColors;    
    }
    void OnDestroy()
    {
        spawnManager.OnQueuedColors -= HandleQueuedColors;    
    }
    void HandleQueuedColors(List<Ball> balls)
    {
        for (int i = 0; i < balls.Count; i ++)
        {
            queuedBallImages[i].color = balls[i].GetSpriteRenderer().color;
            if (balls[i].GetGraphic() != null)
            {
                queuedBallGraphics[i].sprite = balls[i].GetGraphic().sprite;
                switch (balls[i].GetBallType())
                {
                    case BallType.Normal:
                        queuedBallGraphics[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                        break;
                    case BallType.Ghost:
                    case BallType.Diagonal:
                        queuedBallGraphics[i].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                        break;
                    case BallType.Bomb:
                        queuedBallGraphics[i].GetComponent<RectTransform>().sizeDelta = new Vector2(75, 75);
                        break;
                }
            }
        }
    }
}
