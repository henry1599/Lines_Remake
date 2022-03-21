using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Buttons : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private CustomInspectorDisplay customInspectorDisplay;
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
        GameManager.Instance.IsMute = false;
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
    public void OnStartCustomGameButtonClick()
    {
        int width = int.Parse(customInspectorDisplay.InputWidth.text);
        int height = int.Parse(customInspectorDisplay.InputHeight.text);
        int numberOfColors = int.Parse(customInspectorDisplay.InputNumberOfColors.text);
        int ballsEachColor = int.Parse(customInspectorDisplay.InputBallsEachTurn.text);
        int ballsToCollect = int.Parse(customInspectorDisplay.InputBallsToCollect.text);

        width = Mathf.Clamp(width, 6, 30);
        height = Mathf.Clamp(height, 6, 30);
        numberOfColors = Mathf.Clamp(numberOfColors, 1, 7);
        ballsEachColor = Mathf.Clamp(ballsEachColor, 1, 12);
        ballsToCollect = Mathf.Clamp(ballsToCollect, 2, 7);

        SettingManager.Instance.GetLevels()[4] = new Level(GameLevel.Custom, width, height, numberOfColors, ballsEachColor, ballsToCollect);
        SettingManager.Instance.SetLevel(4);
        FindObjectOfType<LevelLoader>().LoadLevelByName("Gameplay");
    }
}
