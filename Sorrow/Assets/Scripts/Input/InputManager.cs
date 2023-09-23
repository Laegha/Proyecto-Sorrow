using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public static Controller controller;
    public static float cameraSensitivity;

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
        controller.Movement.Enable();
        controller.Camera.Enable();
        controller.Dialog.Disable();
        controller.ChaseMovement.Disable();
        controller.ButtonMashing.Disable();
        controller.LockRythm.Disable();
    }

    void OnDisable() => controller.Disable();
}
