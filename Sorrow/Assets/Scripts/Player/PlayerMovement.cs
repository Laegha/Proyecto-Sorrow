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

    void OnEnable()
    {
        InputManager.controller.Player.Enable();
        InputManager.controller.Player.Walk.performed += Walk;
        InputManager.controller.Player.Walk.canceled += StopWalk;
    }

    void OnDisable()
    {
        InputManager.controller.Player.Disable();
        InputManager.controller.Player.Walk.performed -= Walk;
        InputManager.controller.Player.Walk.canceled -= StopWalk;
    }

    public void Walk(InputAction.CallbackContext context)
        => hMovement = context.ReadValue<Vector2>();

    public void StopWalk(InputAction.CallbackContext _)
        => hMovement = Vector3.zero;

    void FixedUpdate() => rb.velocity = transform.TransformDirection(new(hMovement.x, rb.velocity.y, hMovement.y));
}
