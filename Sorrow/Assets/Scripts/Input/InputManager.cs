using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public static Controller controller;
    public float mouseSensitivity;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;
        controller = new Controller();
    }

    void OnEnable()
    {
        controller.Enable();
        controller.Player.Enable();
        controller.Camera.Enable();
        controller.Dialog.Disable();
        controller.PlayerRun.Disable();
        controller.ButtonMashing.Disable();
        controller.LockRythm.Disable();
    }

    void OnDisable() => controller.Disable();
}
