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
        Collider[] colliders = Array.Empty<Collider>();
        var num = Physics.OverlapSphereNonAlloc(transform.position + new Vector3(0f, jumpCheckOffset, 0f), 0.2f, colliders);

        Debug.Log($"Jump, cols num: {num}");

        if (num is not 0 && colliders.Any(x => x.gameObject != gameObject))
            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
    }

    public void Shoot(InputAction.CallbackContext context)
    {

    }
}
