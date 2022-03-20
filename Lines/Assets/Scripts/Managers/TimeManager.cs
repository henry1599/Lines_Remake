using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    private float timer = 0;
    public event Action<float> OnTimeChanged;
    public float Timer 
    {
        get {return timer;}
        set 
        {
            timer = value;
            OnTimeChanged?.Invoke(timer);
        }
    }
    // * 99:59:59
    private const float maxGameTime = 99 * 3600 + 59 * 60 + 59;
    private bool isLock = true;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStateUpdated += HandleGameStateUpdated;
    }
    void OnDestroy()
    {
        GameManager.OnGameStateUpdated -= HandleGameStateUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLock)
        {
            return;
        }
        Timer += Time.deltaTime;
        Timer = Mathf.Clamp(Timer, 0, maxGameTime);
    }
    void HandleGameStateUpdated(GameState state, PlayerData playerData)
    {
        isLock = state != GameState.InGame;
    }
}
