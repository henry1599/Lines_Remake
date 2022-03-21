using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public GameLevel gameLevel;
    [Range(5,30)]
    public int width;
    [Range(5,30)]
    public int height;
    [Range(2,7)]
    public int colorAmount; 
    [Range(2,7)]
    public int queuedBallEachTurn;
    [Range(2,7)]
    public int ballsToCollect;
    public Level(GameLevel _gameLevel, int _width, int _height, int _colorAmount, int _queuedBallEachTurn, int _ballsToCollect = 5)
    {
        gameLevel = _gameLevel;
        width = _width;
        height = _height;
        colorAmount = _colorAmount;
        queuedBallEachTurn = _queuedBallEachTurn;
        ballsToCollect = _ballsToCollect;
    }
}
