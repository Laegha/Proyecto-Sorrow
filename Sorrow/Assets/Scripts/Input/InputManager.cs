using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public static Controller controller;
    public float mouseSensitivity;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;
        controller = new Controller();

        if (TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            controller.Player.Walk.performed += playerMovement.Walk;
            controller.Player.Walk.canceled += playerMovement.StopWalk;
        }

        if (TryGetComponent<PlayerInteraction>(out var playerInteraction))
            controller.Player.Interact.performed += playerInteraction.CheckInteraction;
    }

    void OnEnable() => controller.Enable();
    void OnDisable() => controller.Disable();
}
