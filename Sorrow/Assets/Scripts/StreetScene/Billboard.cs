using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        transform.localScale = transform.parent.localScale / 0.3f;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
            transform.LookAt(mainCamera.transform);
    }
}