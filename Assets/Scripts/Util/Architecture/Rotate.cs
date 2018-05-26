using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
    
    public float xStep = 0;
    public float yStep = 0;
    public float zStep = 10;

    private void FixedUpdate()
    {
        transform.Rotate (xStep, yStep, zStep);
    }
}
