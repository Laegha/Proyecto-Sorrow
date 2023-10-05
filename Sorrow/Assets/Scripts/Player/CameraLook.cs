using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] Animator pointerAnimator;
    Transform player;
    float currXRotation;
    Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = transform.root;
        rb = player.GetComponent<Rigidbody>();
        currXRotation = transform.localRotation.eulerAngles.x;
        if (currXRotation > 180f)
            currXRotation -= 360f;
    }

    void OnEnable() => pointerAnimator.SetBool("Show", true);
    void OnDisable() => pointerAnimator.SetBool("Show", false);

    void Update()
    {
        var delta = InputManager.controller.Camera.Look.ReadValue<Vector2>();
        float mouseY = delta.y * InputManager.cameraSensitivity;
        float mouseX = delta.x * InputManager.cameraSensitivity;

        if (mouseX is not 0f)
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, mouseX, 0f));

        if (mouseY is 0f)
            return;

        currXRotation -= mouseY;
        currXRotation = Mathf.Clamp(currXRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(currXRotation, 0f, 0f);
    }
}
