using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueuedBall : MonoBehaviour
{
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private Image graphic;
    public void Setup(Ball ball)
    {
        spriteRenderer.color = ball.GetSpriteRenderer().color;
        switch (ball.GetBallType())
        {
            case BallType.Normal:
                graphic.enabled = false;
                break;
            case BallType.Ghost:
            case BallType.Diagonal:
                graphic.enabled = true;
                graphic.GetComponent<Image>().sprite = ball.GetGraphic().sprite;
                graphic.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
                break;
            case BallType.Bomb:
                graphic.enabled = true;
                graphic.GetComponent<Image>().sprite = ball.GetGraphic().sprite;
                graphic.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 75);
                break;
        }
    }
}
