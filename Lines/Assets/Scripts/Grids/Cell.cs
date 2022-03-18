using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private int localX;
    [SerializeField] private int localY;

    public int GetLocalX() => localX;
    public int GetLocalY() => localY;

    public void SetLocalPosition(int x, int y)
    {
        localX = x;
        localY = y;
    }
}
