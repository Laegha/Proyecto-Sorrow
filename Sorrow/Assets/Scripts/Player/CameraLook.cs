using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [HideInInspector] public float mouseSensitivity;
    Transform player;

    float currXRotation;
    void Start()
    {
        player = transform.root;
        mouseSensitivity = FindObjectOfType<InputManager>().mouseSensitivity;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if(mouseY != 0)
        {
            currXRotation -= mouseY;
            currXRotation = Mathf.Clamp(currXRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(currXRotation, 0, 0);
        }

        if(mouseX != 0)
            player.Rotate(Vector3.up * mouseX);
    }
}
