using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Buttons : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private GameObject landingPage;
    [SerializeField] private GameObject chooseLevelPage;
    [SerializeField] private Animator animTitleText;
    [SerializeField] private Animator animStartText;
    [SerializeField] private Animator animQuitText;
    [SerializeField] private Animator animChooseLevel;
    public static event Action OnSkipTurnButtonClick;
    public static event Action<string> OnAudioButtonClick;
    public void OnHomeButtonClick()
    {
        if (timeManager == null)
        {
            return;
        }
        if (scoreManager == null)
        {
            return;
        }
        SaveSystem.SavePlayer(scoreManager, timeManager);
        FindObjectOfType<AudioManager>().ForceStopAll();
        FindObjectOfType<LevelLoader>().LoadLevelByName("Home");
    }
    public void OnRestartButtonClick()
    {
        FindObjectOfType<LevelLoader>().LoadLevelByName("Gameplay");
    }
    public void OnPauseButtonClick()
    {
        if (GameManager.Instance.GetGameState() == GameState.Pausing)
        {
            GameManager.Instance.UpdateState(GameState.InGame);
        }
        else if (GameManager.Instance.GetGameState() == GameState.InGame)
        {
            GameManager.Instance.UpdateState(GameState.Pausing);
        }
    }
    public void OnExitButtonClick()
    {
        if (timeManager != null && scoreManager != null)
        {
            SaveSystem.SavePlayer(scoreManager, timeManager);
        }
        Application.Quit();
    }
    public void OnStartButtonClick()
    {
        animTitleText.SetBool("appear", false);
        animStartText.SetBool("appear", false);
        animQuitText.SetBool("appear", false);
        chooseLevelPage.SetActive(true);
    }
    public void OnBackButtonClick()
    {
        // landingPage.SetActive(true);
        animQuitText.SetBool("appear", true);
        animStartText.SetBool("appear", true);
        animTitleText.SetBool("appear", true);
        animChooseLevel.SetBool("appear", false);
        Invoke(nameof(DisableChooseLevelPage),1);
    }
    public void OnLevelButtonClick(int idxLevel)
    {
        SettingManager.Instance.SetLevel(idxLevel);
        FindObjectOfType<LevelLoader>().LoadLevelByName("Gameplay");
    }
    public void OnSkipButtonClick()
    {
        OnSkipTurnButtonClick?.Invoke();
    }
    void DisableChooseLevelPage()
    {
        chooseLevelPage.SetActive(false);
    }
    public void OnAudioClick()
    {
        if (GameManager.Instance.IsMute)
        {
            FindObjectOfType<AudioManager>().UnmuteAll();
            OnAudioButtonClick?.Invoke("ON");
        }
        else
        {
            FindObjectOfType<AudioManager>().MuteAll();
            OnAudioButtonClick?.Invoke("OFF");
        }
        GameManager.Instance.IsMute = !GameManager.Instance.IsMute;
    }
}
