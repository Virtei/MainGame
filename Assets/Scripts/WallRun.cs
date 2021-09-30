using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    Vector3[] directions;
    private bool isWallRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        directions = new Vector3[] { 
            Vector3.right, 
            Vector3.right + Vector3.forward,
            Vector3.forward, 
            Vector3.left + Vector3.forward, 
            Vector3.left
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
