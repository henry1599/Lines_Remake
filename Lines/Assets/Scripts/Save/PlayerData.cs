using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int highScore;
    public int score;
    public float time;
    public PlayerData()
    {
        score = highScore = 0;
    }
    public PlayerData (ScoreManager scoreManager, TimeManager timeManager)
    {
        time = timeManager.Timer;
        highScore = scoreManager.HighScore;
        score = 0;
    }
}
