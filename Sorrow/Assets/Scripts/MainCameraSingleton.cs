using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraSingleton : MonoBehaviour
{
    static MainCameraSingleton instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }
}
