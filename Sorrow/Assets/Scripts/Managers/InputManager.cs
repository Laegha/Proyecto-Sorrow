using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    static public InputManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;
    }
    public KeyCode interactionKey = KeyCode.Mouse0;
    public KeyCode useItemKey = KeyCode.Mouse0;
    public KeyCode dropObjectKey = KeyCode.Q;
    public KeyCode runKey = KeyCode.LeftShift;

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
