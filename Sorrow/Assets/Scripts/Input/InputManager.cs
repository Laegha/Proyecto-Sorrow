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

        controller.Player.Enable();
        controller.Dialog.Disable();
        controller.PlayerRun.Disable();
        
        if (TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            controller.Player.Walk.performed += playerMovement.Walk;
            controller.Player.Walk.canceled += playerMovement.StopWalk;
        }

        if (TryGetComponent<PlayerInteraction>(out var playerInteraction))
            controller.Player.Interact.performed += playerInteraction.CheckInteraction;

        if (TryGetComponent<PlayerChaseMovement>(out var playerChaseMovement))
        {
            controller.PlayerRun.Jump.performed += playerChaseMovement.Jump;
            controller.Player.UseItem.performed += playerChaseMovement.Shoot;
        }
    }

    void OnEnable() => controller.Enable();
    void OnDisable() => controller.Disable();
}
