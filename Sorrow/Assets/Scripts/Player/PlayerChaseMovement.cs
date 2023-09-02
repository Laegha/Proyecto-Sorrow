using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChaseMovement : MonoBehaviour
{
    [SerializeField] float fForce;
    [SerializeField] float airDrag;
    [SerializeField] float stopDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCheckOffset;
    [SerializeField] float jumpBufferTime;
    [SerializeField] float airControl;
    Vector3 input;
    bool canJump;
    float jumpBuffer;
    readonly Vector3 halfExtents = new(0.45f, 0.1f, 0.45f);
    Rigidbody rb;
    HeldObjectManager heldObjectManager;
    PlayerMovement playerMovement;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        heldObjectManager = GetComponent<HeldObjectManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        playerMovement.enabled = false;
        InputManager.controller.PlayerRun.Enable();
        InputManager.controller.PlayerRun.Jump.performed += Jump;
        InputManager.controller.PlayerRun.Run.performed += Run;
        InputManager.controller.PlayerRun.Run.canceled += StopRun;
        InputManager.controller.PlayerRun.Shoot.performed += heldObjectManager.UseObject;

        OnCollisionEnter(default);
    }

    void OnDisable()
    {
        InputManager.controller.PlayerRun.Jump.performed -= Jump;
        InputManager.controller.PlayerRun.Run.performed -= Run;
        InputManager.controller.PlayerRun.Run.canceled -= StopRun;
        InputManager.controller.PlayerRun.Shoot.performed -= heldObjectManager.UseObject;
        InputManager.controller.PlayerRun.Disable();
        playerMovement.enabled = true;
        
        rb.drag = 0f;
    }

    void Jump(InputAction.CallbackContext _)
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

    void Run(InputAction.CallbackContext context)
    {
        var vector2 = context.ReadValue<Vector2>();
        input = new(vector2.x, 0f, vector2.y);
    }

    void StopRun(InputAction.CallbackContext _) => input = Vector3.zero;

    void Update() => jumpBuffer -= Time.deltaTime;

    void FixedUpdate()
    {
        Vector3 finalVelocity = input;

        //bool isGoingOpositeZ = Mathf.Abs(rb.velocity.z) > 0.1f && Mathf.Sign(transform.TransformDirection(input).z) != Mathf.Sign(rb.velocity.z);
        bool airborne = rb.velocity.y != 0f && !CheckGround();

        /*
        if (airborne && isGoingOpositeZ)
            if (rb.velocity.y < -.1f)
                finalVelocity.z *= -airControl;
            else
                return;
        */

        if (airborne)
        {
            rb.drag = airDrag;
            finalVelocity *= airControl;
        }
        else
            rb.drag = stopDrag;

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
        => Physics.OverlapBoxNonAlloc(transform.position + new Vector3(0f, jumpCheckOffset, 0f), halfExtents, null, Quaternion.identity, 0) is not 0;

    void ApplyForceToReachVelocity(Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force is 0f || velocity.magnitude is 0f)
            return;

        velocity += 0.2f * rb.drag * velocity.normalized;

        force = Mathf.Clamp(force, -rb.mass / Time.fixedDeltaTime, rb.mass / Time.fixedDeltaTime);

        if (rb.velocity.magnitude is 0f)
        {
            rb.AddForce(velocity * force, mode);
            return;
        }
        
        var velocityProjectedToTarget = velocity.normalized * Vector3.Dot(velocity, rb.velocity) / velocity.magnitude;
        rb.AddForce((velocity - velocityProjectedToTarget) * force, mode);
    }
}
