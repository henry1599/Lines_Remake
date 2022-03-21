using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance {get; set;}
    private float orthoOrg;
    private float orthoCurr;
    private Vector3 scaleOrg;
    private Vector3 posOrg;
    public GameObject clickEffect;
    void Awake() 
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Cursor.visible = false;    
    }
    void Start()
    {
        orthoOrg = Camera.main.orthographicSize;
        orthoCurr = orthoOrg;
        scaleOrg = transform.localScale;
        posOrg = Camera.main.WorldToViewportPoint(transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPosition = Helper.Camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(clickEffect, transform.position, Quaternion.identity);
        }
        var osize = Camera.main.orthographicSize;
        if (orthoCurr != osize)
        {
            transform.localScale = scaleOrg * osize / orthoOrg;
            orthoCurr = osize;
            // transform.position = Camera.main.ViewportToWorldPoint(posOrg);
        }
    }
}
