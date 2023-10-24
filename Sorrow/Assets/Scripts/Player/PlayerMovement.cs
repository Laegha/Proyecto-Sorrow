using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 hMovement;
    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void OnEnable() => StartCoroutine(SubscribeEvents());
    
    IEnumerator SubscribeEvents()
    {
        yield return new WaitForEndOfFrame();

        InputManager.controller.Movement.Enable();
        InputManager.controller.Movement.Walk.performed += Walk;
        InputManager.controller.Movement.Walk.canceled += StopWalk;
    }

    void OnDisable()
    {
        InputManager.controller.Movement.Disable();
        InputManager.controller.Movement.Walk.performed -= Walk;
        InputManager.controller.Movement.Walk.canceled -= StopWalk;
    }

    public void Walk(InputAction.CallbackContext context)
        => hMovement = context.ReadValue<Vector2>();

    public void StopWalk(InputAction.CallbackContext _)
        => hMovement = Vector3.zero;

    void FixedUpdate() => rb.velocity = transform.TransformDirection(new(hMovement.x, rb.velocity.y, hMovement.y));
}
