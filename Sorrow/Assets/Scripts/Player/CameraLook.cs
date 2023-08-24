using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    Transform player;
    float currXRotation;
    Rigidbody rb;
    bool transformVelocity;

    void Start()
    {
        player = transform.root;
        Cursor.lockState = CursorLockMode.Locked;
        rb = player.GetComponent<Rigidbody>();
        transformVelocity = player.TryGetComponent<PlayerChaseMovement>(out _);
    }

    void Update()
    {
        var delta = InputManager.controller.Camera.Look.ReadValue<Vector2>();
        float mouseY = delta.y * InputManager.instance.mouseSensitivity;
        float mouseX = delta.x * InputManager.instance.mouseSensitivity;

        if (mouseX != 0)
        {
            var quaternion = Quaternion.Euler(0f, mouseX, 0f);
            rb.MoveRotation(rb.rotation * quaternion);

            if (transformVelocity)
                rb.velocity = quaternion * rb.velocity;
        }   

        if (mouseY == 0)
            return;

        currXRotation -= mouseY;
        currXRotation = Mathf.Clamp(currXRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(currXRotation, 0, 0);
    }
}
