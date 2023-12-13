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
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        controller = new Controller();
        cameraSensitivity = PlayerPrefs.GetFloat("CS", 100f) * .001f;
        playerMovement = GetComponent<PlayerMovement>();
        cameraLook = GetComponentInChildren<CameraLook>();
        heldObjectManager = GetComponent<HeldObjectManager>();
    }

    void OnEnable()
    {
        if (!gameObject || instance != this)
            return;

        controller.Enable();
        controller.Movement.Enable();
        controller.Camera.Enable();
        controller.Dialog.Disable();
        controller.ChaseMovement.Disable();
        controller.ButtonMashing.Disable();
        controller.LockRhythm.Disable();
    }

    void OnDisable()
    {
        if (gameObject && instance == this)
            controller.Disable();
    }

    public void RemRegControl(bool enablement)
    {
        if (!playerMovement || !cameraLook || !heldObjectManager)
            return;

        playerMovement.enabled = enablement;
        cameraLook.enabled = enablement;
        heldObjectManager.enabled = enablement;
    }
}
