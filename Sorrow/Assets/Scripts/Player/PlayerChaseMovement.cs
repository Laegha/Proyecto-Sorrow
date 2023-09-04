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
    [SerializeField] float groundDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCheckOffset;
    [SerializeField] float jumpBufferTime;
    [SerializeField] float coyoteTime;
    [SerializeField] float airControl;
    Vector3 input;
    bool grounded;
    float jumpBuffer;
    float coyoteBuffer;
    readonly Vector3 halfExtentsEnter = new(0.45f, 0.05f, 0.45f);
    readonly Vector3 halfExtentsExit = new(0.1f, 0.05f, 0.1f);
    LayerMask maskToIgnore = ~(1 << 7);
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
        if (!grounded && coyoteBuffer < 0f)
        {
            jumpBuffer = jumpBufferTime;
            return;
        }

        // 1) Darle la fuerza y sacar la capacidad de saltar para que no pueda volver a saltar hasta realmente tocar el piso.
        rb.velocity.Set(rb.velocity.x, 0f, rb.velocity.z);
        rb.drag = airDrag;
        rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
        grounded = false;
    }

    void Run(InputAction.CallbackContext context)
    {
        var vector2 = context.ReadValue<Vector2>();
        input = new(vector2.x, 0f, vector2.y);
    }

    void StopRun(InputAction.CallbackContext _) => input = Vector3.zero;

    void Update()
    {
        jumpBuffer -= Time.deltaTime;
        coyoteBuffer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector3 finalVelocity = input;

        if (!grounded)
            finalVelocity *= airControl;

        ApplyForceToReachVelocity(transform.TransformDirection(finalVelocity), fForce);
    }

    // 2) Al entrar en colisión con algo, fijarse si es el piso o no. Eso determinará si puede saltar o no.
    void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return;

        grounded = CheckGround(halfExtentsEnter);

        rb.drag = grounded ? groundDrag : airDrag;

        if (grounded && jumpBuffer > 0f)
            Jump(default);
    }

    void OnCollisionExit(Collision collision)
    {
        if (!grounded || !enabled) return;

        grounded = CheckGround(halfExtentsExit);

        print(grounded);

        rb.drag = grounded ? groundDrag : airDrag;

        if (!grounded)
            coyoteBuffer = coyoteTime;
    }

    bool CheckGround(Vector3 halfExtents)
        => Physics.OverlapBoxNonAlloc(transform.position + new Vector3(0f, jumpCheckOffset, 0f), halfExtents, new Collider[16], Quaternion.identity, maskToIgnore) is not 0;

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
