using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    static readonly Vector3 standardScale = new(.4f, .4f, .4f);

    void Start()
    { 
        mainCamera = Camera.main;
        transform.SetParent(null);
        transform.localScale = standardScale;
    }
    void Update()
    {
        if (mainCamera != null)
            transform.LookAt(mainCamera.transform);
    }
}