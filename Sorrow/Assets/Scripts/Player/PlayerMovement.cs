using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector3 hMovement;
    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    public void Walk(InputAction.CallbackContext context)
    {
        var vector2 = context.ReadValue<Vector2>();
        hMovement = new Vector3(vector2.x, 0, vector2.y);
    }

    public void StopWalk(InputAction.CallbackContext context)
        => hMovement = Vector3.zero;

    void FixedUpdate() => rb.velocity = transform.TransformDirection(hMovement);
}
