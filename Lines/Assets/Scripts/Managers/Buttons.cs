using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
