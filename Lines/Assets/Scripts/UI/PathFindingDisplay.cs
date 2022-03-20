using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingDisplay : MonoBehaviour
{
    [SerializeField] private GameObject vfxCannotMove;
    [SerializeField] private PathFinding pathFinding;
    // Start is called before the first frame update
    void Start()
    {
        pathFinding.OnPathFound += HandlePathFound;
    }
    void OnDestroy()
    {
        pathFinding.OnPathFound -= HandlePathFound;
    }
    void HandlePathFound(bool found)
    {
        if (found == false)
        {
            Instantiate(vfxCannotMove, transform.position, Quaternion.identity);
        }
    }

}
