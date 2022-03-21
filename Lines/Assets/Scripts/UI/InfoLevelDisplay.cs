using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoLevelDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text widthText;
    [SerializeField] private TMP_Text heightText;
    [SerializeField] private TMP_Text numberOfColorsText;
    [SerializeField] private TMP_Text ballsEachTurnText;
    [SerializeField] private TMP_Text ballsToCollectText;
    public void Setup(Level level)
    {
        widthText.text = $"Width : {level.width.ToString()}";
        heightText.text = $"Height : {level.height.ToString()}";
        numberOfColorsText.text = $"Number of colors : {level.colorAmount.ToString()}";
        ballsEachTurnText.text = $"Balls each turn : {level.queuedBallEachTurn.ToString()}";
        ballsToCollectText.text = $"Balls to collect : {level.ballsToCollect.ToString()}";
    }
    public void SetupIngame(Level level)
    {
        widthText.text = level.width.ToString();
        heightText.text = level.height.ToString();
        numberOfColorsText.text = level.colorAmount.ToString();
        ballsEachTurnText.text = level.queuedBallEachTurn.ToString();
        ballsToCollectText.text = level.ballsToCollect.ToString();
    }
}
