using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PauseDisplay : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private TMP_Text pauseText;
    void Start()
    {
        GameManager.OnGameStateUpdated += HandleGameStateUpdated;
    }
    void OnDestroy()
    {
        GameManager.OnGameStateUpdated -= HandleGameStateUpdated;
    }
    void HandleGameStateUpdated(GameState state, PlayerData playerData)
    {
        anim.SetBool("pause", state == GameState.Pausing);
        pauseText.text = state == GameState.Pausing ? "CONTINUE" : "PAUSE";
    }
}
