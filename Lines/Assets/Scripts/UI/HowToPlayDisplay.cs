using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HowToPlayDisplay : MonoBehaviour
{
    private const string normalBallMessage = "Only move in 4 directions and cannot overlap with others";
    private const string ghostBallMessage = "Only move in 4 directions and can overlap with others";
    private const string diagonalBallMessage = "Can move diagonally and cannot overlap with others";
    private const string bombBallMessage = "Like the normal ball but explore when stop moving";
    [SerializeField] private TMP_Text message;
    [SerializeField] private GameObject infoField;
    // Start is called before the first frame update
    void Start()
    {
        Ball.OnHover += HandleHover;
        Ball.OnExit += HandleExit;
    }
    void OnDestroy()
    {
        Ball.OnHover -= HandleHover;
        Ball.OnExit -= HandleExit;
    }
    void HandleHover(Ball ball)
    {
        switch (ball.GetBallType())
        {
            case BallType.Normal:
                message.text = normalBallMessage;
                break;
            case BallType.Ghost:
                message.text = ghostBallMessage;
                break;
            case BallType.Diagonal:
                message.text = diagonalBallMessage;
                break;
            case BallType.Bomb:
                message.text = bombBallMessage;
                break;
        }
        infoField.SetActive(true);
    }
    void HandleExit()
    {
        message.text = "";
        infoField.SetActive(false);
    }
}
