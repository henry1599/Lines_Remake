using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private TMP_Text timeText = null;
    void Start()
    {
        timeManager.OnTimeChanged += HandleTimeChanged;
    }
    void OnDestroy()
    {
        timeManager.OnTimeChanged -= HandleTimeChanged;
    }
    void HandleTimeChanged(float time)
    {
        timeText.text = GetTimeString(time);
    }
    Tuple<int, int, int> SplitFloatToTime(float _timer)
    {
        int timerInt = (int)_timer;

        int hour = timerInt / 3600;
        int minute = (timerInt - hour * 3600) / 60;
        int second = timerInt - hour * 3600 - minute * 60;

        return new Tuple<int, int, int>(hour, minute, second);
    }
    public string GetTimeString(float time)
    {
        Tuple<int, int, int> splitedTime = SplitFloatToTime(time);
        int hour = splitedTime.Item1;
        int minute = splitedTime.Item2;
        int second = splitedTime.Item3;

        string hourString = hour > 10 ? hour.ToString() : $"0{hour}";
        string minuteString = minute > 10 ? minute.ToString() : $"0{minute}";
        string secondString = second > 10 ? second.ToString() : $"0{second}";

        return $"{hourString}:{minuteString}:{secondString}";
    }
}
