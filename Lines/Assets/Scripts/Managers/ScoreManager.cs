using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; set;}
    [SerializeField] private Animator scoreAnim;
    [SerializeField] private Animator highScoreAnim;
    private int highScore;
    private int score;
    public static event Action<int, int> OnScoreUpdated;
    void Awake()
    {
        Instance = this;
    }
    public int Score
    {
        get {return score;}
        set
        {
            score = value;
            scoreAnim.SetTrigger("changed");
            if (score > highScore)
            {
                HighScore = score;
            }
            OnScoreUpdated?.Invoke(score, highScore);
        }
    }
    public int HighScore 
    {
        get {return highScore;}
        set 
        {
            highScore = value;
            highScoreAnim.SetTrigger("changed");
        }
    }
}
