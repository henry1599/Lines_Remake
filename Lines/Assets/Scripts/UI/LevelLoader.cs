using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public void LoadLevelByName(string name)
    {
        StartCoroutine(LoadLevel(name));
    }
    IEnumerator LoadLevel(string name)
    {
        anim.SetTrigger("start");
        yield return Helper.GetWait(1);
        SceneManager.LoadScene(name);
    }
}
