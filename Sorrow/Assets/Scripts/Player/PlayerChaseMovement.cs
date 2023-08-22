using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChaseMovement : MonoBehaviour
{
    [SerializeField] float burstAccelMult;
    [SerializeField] float maxBurstSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float moveDrag;
    [SerializeField] float stopDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCheckOffset;
    [SerializeField] float jumpBufferTime;
    Rigidbody rb;
    Vector2 accel;
    bool canJump;
    float jumpBuffer;
    float previousMaxSpeed;
    readonly Vector3 halfExtents = new(0.45f, 0.1f, 0.45f);

    void Awake() => rb = GetComponent<Rigidbody>();

    void OnEnable()
    {
        InputManager.controller.Player.Disable();
        InputManager.controller.PlayerRun.Enable();
        rb.drag = stopDrag;
        previousMaxSpeed = rb.maxLinearVelocity;
        rb.maxLinearVelocity = maxSpeed;
        OnCollisionEnter(default);
    }

    void OnDisable()
    {
        InputManager.controller.Player.Enable();
        InputManager.controller.PlayerRun.Disable();
        rb.drag = 0f;
        rb.maxLinearVelocity = previousMaxSpeed;
    }

    public void Jump(InputAction.CallbackContext context)
    {   
        // 3) Si no puede saltar o no está tocando el piso, no hacer nada.
        if (!canJump || !CheckGround())
        {
            jumpBuffer = jumpBufferTime;
            return;
        }

        // 1) Darle la fuerza y sacar la capacidad de saltar para que no pueda volver a saltar hasta realmente tocar el piso.
        rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
        canJump = false;
    }

    public void Run(InputAction.CallbackContext context)
    {
        accel = context.ReadValue<Vector2>();
        rb.drag = moveDrag;
    }

    public void StopRun(InputAction.CallbackContext context)
    {
        accel = Vector2.zero;
        if (CheckGround())
            rb.drag = stopDrag;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("SHOOT");
    }

    void Update()
    {
        Debug.Log("V " + rb.velocity.magnitude + " " + rb.GetPointVelocity(transform.position));

        jumpBuffer -= Time.deltaTime;

        if (Mathf.Abs(rb.velocity.y) > .5f)
            rb.drag = moveDrag;

        if (accel == Vector2.zero)
            return;

        var vector = accel;

        if (rb.velocity.magnitude < maxBurstSpeed)
            vector *= burstAccelMult;

        rb.AddRelativeForce(vector.x , 0, vector.y, ForceMode.Acceleration);
    }

    // 2) Al entrar en colisión con algo, fijarse si es el piso o no. Eso determinará si puede saltar o no.
    void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return;

        canJump = CheckGround();

        if (!canJump) return;

        if (jumpBuffer > 0f)
            Jump(default);
        
        if (accel == Vector2.zero)
            rb.drag = stopDrag;
    }

    bool CheckGround()
        => Physics.OverlapBox(transform.position + new Vector3(0f, jumpCheckOffset, 0f), halfExtents).Length is not 0;
}
