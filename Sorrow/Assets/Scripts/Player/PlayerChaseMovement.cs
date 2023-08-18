using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChaseMovement : MonoBehaviour
{
    [SerializeField] float sprintMultiplier;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCheckOffset;
    InputBinding speedOverride;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speedOverride = new InputBinding(){
            groups = "Keyboard&Mouse",
            overrideProcessors = $",ScaleVector2(x={sprintMultiplier},y={sprintMultiplier})"
        };
    }

    void OnEnable()
    {
        InputManager.controller.Player.Walk.ApplyBindingOverride(speedOverride);
        Debug.Log(InputManager.controller.Player.Walk.processors);
    }

    void OnDisable()
    {
        InputManager.controller.Player.Walk.RemoveBindingOverride(speedOverride);
        Debug.Log(InputManager.controller.Player.Walk.processors);
    }

    public void Jump(InputAction.CallbackContext context)
    {   
        var cols = Physics.OverlapSphere(transform.position + new Vector3(0f, jumpCheckOffset, 0f), 0.2f);

        Debug.Log($"Jump, cols num: {cols}");

        if (cols.Length is not 0)
            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
    }
    
    public void Run(InputAction.CallbackContext context)
    {
        Debug.Log("RUN");
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("SHOOT");
    }
}
