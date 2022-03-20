using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text audioStatus;
    void Start()
    {
        Buttons.OnAudioButtonClick += HandleAudioButtonClick;
    }
    void OnDestroy()
    {
        Buttons.OnAudioButtonClick -= HandleAudioButtonClick;
    }
    void HandleAudioButtonClick(string status)
    {
        audioStatus.text = status;
    }
}
