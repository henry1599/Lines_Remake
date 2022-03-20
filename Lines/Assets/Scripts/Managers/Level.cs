using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public GameLevel gameLevel;
    [Range(5,20)]
    public int width;
    [Range(5,20)]
    public int height;
    [Range(2,7)]
    public int colorAmount; 
    [Range(2,7)]
    public int queuedBallEachTurn;
}
