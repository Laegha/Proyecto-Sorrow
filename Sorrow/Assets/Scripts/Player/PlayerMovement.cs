using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 hMovement;
    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    public void Walk(InputAction.CallbackContext context)
        => hMovement = context.ReadValue<Vector2>();

    public void StopWalk(InputAction.CallbackContext _)
        => hMovement = Vector3.zero;

    void FixedUpdate() => rb.velocity = transform.TransformDirection(new(hMovement.x, rb.velocity.y, hMovement.y));
}
