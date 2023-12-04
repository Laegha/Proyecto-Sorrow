using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    static readonly Vector3 standardScale = new(.4f, .4f, .4f);
    static GameObject bullseyeParent;

    void Start()
    { 
        mainCamera = Camera.main;
        if (!bullseyeParent)
        {
            bullseyeParent = GameObject.Find("BullseyeParent");
            bullseyeParent.SetActive(false);
        }

        transform.SetParent(bullseyeParent.transform);
        transform.localScale = standardScale;
    }
    void Update()
    {
        if (mainCamera != null)
            transform.LookAt(mainCamera.transform);
    }
}