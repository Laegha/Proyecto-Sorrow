using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public int mouseSensitivity;
    public int MouseSensitivity
    {
        get { return mouseSensitivity; }
        set
        {
            mouseSensitivity = value;
            CameraLook camera = FindObjectOfType<CameraLook>();
            if(camera != null ) camera.mouseSensitivity= mouseSensitivity;
        }
    }
}
