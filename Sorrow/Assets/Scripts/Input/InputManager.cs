using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public static Controller controller;
    public static float cameraSensitivity;

    PlayerMovement playerMovement;
    CameraLook cameraLook;
    HeldObjectManager heldObjectManager;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;
        controller = new Controller();
        cameraSensitivity = PlayerPrefs.GetFloat("CS", 100f) * .001f;
        playerMovement = GetComponent<PlayerMovement>();
        cameraLook = GetComponent<CameraLook>();
        heldObjectManager = GetComponent<HeldObjectManager>();
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

    public void RemRegControl(bool enablement)
    {
        playerMovement.enabled = enablement;
        cameraLook.enabled = enablement;
        heldObjectManager.enabled = enablement;
    }
}
