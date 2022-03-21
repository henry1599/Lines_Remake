using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class CustomInspectorDisplay : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputWidth;
    [SerializeField] private TMP_InputField inputHeight;
    [SerializeField] private TMP_InputField inputNumberOfColors;
    [SerializeField] private TMP_InputField inputBallsEachTurn;
    [SerializeField] private TMP_InputField inputBallsToCollect;
    [SerializeField] private Button startButton; 
    private int resultWidth; 
    private int resultHeight; 
    private int resultNumberOfColors; 
    private int resultBallsEachTurn;
    private int resultBallsToCollect;
    public Button StartButton
    {
        get {return startButton;}
        set {startButton = value;}
    }
    public TMP_InputField InputWidth
    {
        get {return inputWidth;}
        set {inputWidth = value;}
    }
    public TMP_InputField InputHeight
    {
        get {return inputHeight;}
        set {inputHeight = value;}
    }
    public TMP_InputField InputNumberOfColors
    {
        get {return inputNumberOfColors;}
        set {inputNumberOfColors = value;}
    }
    public TMP_InputField InputBallsEachTurn
    {
        get {return inputBallsEachTurn;}
        set {inputBallsEachTurn = value;}
    }
    public TMP_InputField InputBallsToCollect
    {
        get {return inputBallsToCollect;}
        set {inputBallsToCollect = value;}
    }
    public void InputWidthOnValueChanged()
    {
        if (!int.TryParse(InputWidth.text, out int result))
        {
            StartButton.interactable = false;
        }
        else
        {
            StartButton.interactable = true;
            resultWidth = Mathf.Clamp(result, 6, 30);
        }
    }
    public void InputHeightOnValueChanged()
    {
        if (!int.TryParse(InputHeight.text, out int result))
        {
            StartButton.interactable = false;
        }
        else
        {
            StartButton.interactable = true;
            resultHeight = Mathf.Clamp(result, 6, 30);
        }
    }
    public void InputNumberOfColorsOnValueChanged()
    {
        if (!int.TryParse(InputNumberOfColors.text, out int result))
        {
            StartButton.interactable = false;
        }
        else
        {
            StartButton.interactable = true;
            resultNumberOfColors = Mathf.Clamp(result, 1, 7);
        }
    }
    public void InputBallsEachTurnOnValueChanged()
    {
        if (!int.TryParse(InputBallsEachTurn.text, out int result))
        {
            StartButton.interactable = false;
        }
        else
        {
            StartButton.interactable = true;
            resultBallsEachTurn = Mathf.Clamp(result, 1, 12);
        }
    }
    public void InputBallsToCollectOnValueChanged()
    {
        if (!int.TryParse(InputBallsToCollect.text, out int result))
        {
            StartButton.interactable = false;
        }
        else
        {
            StartButton.interactable = true;
            resultBallsToCollect = Mathf.Clamp(result, 2, 7);
        }
    }
    public void InputWidthDeselect()
    {
        InputWidth.text = resultWidth.ToString();
    }
    public void InputHeightDeselect()
    {
        InputHeight.text = resultHeight.ToString();
    }
    public void InputNumberOfColorsDeselect()
    {
        InputNumberOfColors.text = resultNumberOfColors.ToString();
    }
    public void InputBallsEachTurnDeselect()
    {
        InputBallsEachTurn.text = resultBallsEachTurn.ToString();
    }
    public void InputBallsToCollectDeselect()
    {
        InputBallsToCollect.text = resultBallsToCollect.ToString();
    }
}
