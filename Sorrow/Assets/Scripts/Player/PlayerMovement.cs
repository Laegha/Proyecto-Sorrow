using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 hMovement;

    public void Walk(InputAction.CallbackContext context) 
        => hMovement = context.ReadValue<Vector2>();

    public void StopWalk(InputAction.CallbackContext context)
        => hMovement = Vector2.zero;

    void Update()
    {
        var vector = hMovement * Time.deltaTime;

        if (vector != Vector2.zero)
            transform.Translate(vector.x, 0, vector.y);
    }
}
