using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    void Start() => mainCamera = Camera.main;

    void Update()
    {
        if (mainCamera != null)
            transform.LookAt(mainCamera.transform);
    }
}