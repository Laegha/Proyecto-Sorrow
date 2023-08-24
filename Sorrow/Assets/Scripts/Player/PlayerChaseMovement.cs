using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChaseMovement : MonoBehaviour
{
    [SerializeField] float fForce;
    [SerializeField] float moveDrag;
    [SerializeField] float stopDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCheckOffset;
    [SerializeField] float jumpBufferTime;
    [SerializeField] float airControl;
    Rigidbody rb;
    Vector3 input;
    bool canJump;
    float jumpBuffer;
    readonly Vector3 halfExtents = new(0.45f, 0.1f, 0.45f);

    void Awake() => rb = GetComponent<Rigidbody>();

    void OnEnable()
    {
        InputManager.controller.Player.Disable();
        InputManager.controller.PlayerRun.Enable();
        rb.drag = stopDrag;
        OnCollisionEnter(default);
    }

    void OnDisable()
    {
        InputManager.controller.Player.Enable();
        InputManager.controller.PlayerRun.Disable();
        rb.drag = 0f;
    }

    public void Jump(InputAction.CallbackContext _)
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
        var vector2 = context.ReadValue<Vector2>();
        input = new(vector2.x, 0f, vector2.y);
        rb.drag = moveDrag;
    }

    public void StopRun(InputAction.CallbackContext _)
    {
        input = Vector3.zero;
        if (CheckGround())
            rb.drag = stopDrag;
    }

    void Update()
    {
        //Debug.Log("V " + rb.velocity.magnitude + " " + rb.GetPointVelocity(transform.position));

        jumpBuffer -= Time.deltaTime;
        
        if (Mathf.Abs(rb.velocity.y) > .5f)
            rb.drag = moveDrag;
    }

    void FixedUpdate()
    {
        Vector3 finalVelocity = input;

        bool isGoingOpositeZ = Mathf.Abs(rb.velocity.z) > 0.1f && Mathf.Sign(transform.TransformDirection(input).z) != Mathf.Sign(rb.velocity.z);
        bool airborne = rb.velocity.y != 0f && !CheckGround();

        if (airborne && isGoingOpositeZ)
            if (rb.velocity.y < -.1f)
                finalVelocity.z *= -airControl;
            else
                return;

        ApplyForceToReachVelocity(transform.TransformDirection(finalVelocity), fForce);
    }

    // 2) Al entrar en colisión con algo, fijarse si es el piso o no. Eso determinará si puede saltar o no.
    void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return;

        canJump = CheckGround();

        if (!canJump) return;

        if (jumpBuffer > 0f)
            Jump(default);
        
        if (input == Vector3.zero)
            rb.drag = stopDrag;
    }

    bool CheckGround()
        => Physics.OverlapBox(transform.position + new Vector3(0f, jumpCheckOffset, 0f), halfExtents).Length is not 0;

    void ApplyForceToReachVelocity(Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity += 0.2f * rb.drag * velocity.normalized;

        force = Mathf.Clamp(force, -rb.mass / Time.fixedDeltaTime, rb.mass / Time.fixedDeltaTime);

        if (rb.velocity.magnitude == 0)
        {
            rb.AddForce(velocity * force, mode);
            return;
        }
        
        var velocityProjectedToTarget = velocity.normalized * Vector3.Dot(velocity, rb.velocity) / velocity.magnitude;
        Debug.Log("Vel " + velocity + " VP " + velocityProjectedToTarget);
        rb.AddForce((velocity - velocityProjectedToTarget) * force, mode);
    }
}
