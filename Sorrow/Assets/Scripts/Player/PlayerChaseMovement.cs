using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChaseMovement : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCheckOffset;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    Vector2 force;
    bool canJump;
    readonly Vector3 halfExtents = new(0.45f, 0.1f, 0.45f);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        InputManager.controller.Player.Disable();
        InputManager.controller.PlayerRun.Enable();
        rb.drag = 0.2f;
    }

    void OnDisable()
    {
        InputManager.controller.Player.Enable();
        InputManager.controller.PlayerRun.Disable();
        rb.drag = 0f;
    }

    public void Jump(InputAction.CallbackContext context)
    {   
        // 3) Si no puede saltar o no está tocando el piso, no hacer nada.
        if (!canJump || !CheckGround())
            return;

        // 1) Darle la fuerza y sacar la capacidad de saltar para que no pueda volver a saltar hasta realmente tocar el piso.
        rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
        canJump = false;
    }

    public void Run(InputAction.CallbackContext context)
        => force = context.ReadValue<Vector2>();

    public void StopRun(InputAction.CallbackContext context)
        => force = Vector2.zero;

    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("SHOOT");
    }

    void Update()
    {
        Debug.Log(rb.velocity.magnitude);
        if (force == Vector2.zero)
            return;

        var vector = force * Time.deltaTime;

        rb.AddRelativeForce(vector.x, 0, vector.y, ForceMode.Force);
    }

    // 2) Al entrar en colisión con algo, fijarse si es el piso o no. Eso determinará si puede saltar o no.
    void OnCollisionEnter(Collision collision) => canJump = CheckGround();

    bool CheckGround()
        => Physics.OverlapBox(transform.position + new Vector3(0f, jumpCheckOffset, 0f), halfExtents).Length is not 0;
}
