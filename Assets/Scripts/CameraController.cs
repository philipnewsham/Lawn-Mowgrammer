using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraDolly;

    public void SetCamera(float x, float z)
    {
        cameraDolly.position = new Vector3(x / 2, 0.0f, z / 2);
        Camera.main.orthographicSize = (x > z ? x : z);
    }
}
