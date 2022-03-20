using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance {get; set;}
    [SerializeField] private Level[] levels = new Level[4];
    private Level chosenLevel = null;
    public Level ChosenLevel 
    {
        get {return chosenLevel;}
        set {chosenLevel = value;}
    }
    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SetLevel(int idxLevel)
    {
        chosenLevel = levels[idxLevel];
    }
}
