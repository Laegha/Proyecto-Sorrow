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

        if (TryGetComponent<HeldObjectManager>(out var heldObjectManager))
        {
            // Suscribe to event before check interaction to prevent pickup and use overlapping in the same click (I'm a god)
            controller.Player.Click.performed += heldObjectManager.UseObject;
            controller.Player.Drop.performed += heldObjectManager.DropObject;
        }

        if (TryGetComponent<PlayerInteraction>(out var playerInteraction))
            controller.Player.Click.performed += playerInteraction.CheckInteraction;

        if (TryGetComponent<PlayerChaseMovement>(out var playerChaseMovement))
        {
            controller.PlayerRun.Run.performed += playerChaseMovement.Run;
            controller.PlayerRun.Run.canceled += playerChaseMovement.StopRun;
            controller.PlayerRun.Jump.performed += playerChaseMovement.Jump;
            controller.PlayerRun.Shoot.performed += heldObjectManager.UseObject;
        }

        if (TryGetComponent<ButtonMashing>(out var buttonMashing))
            controller.ButtonMashing.Button.performed += buttonMashing.Mash;

        if (TryGetComponent<LockRythmController>(out var lockRythmController))
            controller.LockRythm.LockNum.performed += lockRythmController.Lock;
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
