using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    Transform player;
    float currXRotation;

    void Start()
    {
        player = transform.root;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var delta = InputManager.controller.Player.Camera.ReadValue<Vector2>();
        float mouseY = delta.y * InputManager.instance.mouseSensitivity;
        float mouseX = delta.x * InputManager.instance.mouseSensitivity;

        if (mouseY != 0)
        {
            currXRotation -= mouseY;
            currXRotation = Mathf.Clamp(currXRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(currXRotation, 0, 0);
        }

        if(mouseX != 0)
            player.Rotate(Vector3.up * mouseX);
    }
}
