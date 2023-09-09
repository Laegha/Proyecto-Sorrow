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
    [SerializeField] float jumpBufferTime;
    [SerializeField] float coyoteTime;
    [SerializeField] float airControl;
    [SerializeField] float maxSlopeAngle;
    Vector3 input;
    Vector3 slopeNormal;
    bool grounded;
    float jumpBuffer;
    float coyoteBuffer;
    readonly Vector3 checkOffsetEnter = new(0f, -1.1f, 0f);
    readonly Vector3 checkOffsetExit = new(0f, -1.05f, 0f);
    readonly Vector3 rayCastOffset = new(0f, -1f, 0f);
    readonly Vector3 halfExtentsEnter = new(.45f, .05f, .45f);
    readonly Vector3 halfExtentsExit = new(.1f, .025f, .1f);
    readonly LayerMask maskToIgnore = ~(1 << 7);
    //readonly LayerMask slopeMask = 1 << 9;
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
        InputManager.controller.ChaseMovement.Enable();
        InputManager.controller.ChaseMovement.Jump.performed += Jump;
        InputManager.controller.ChaseMovement.Run.performed += Run;
        InputManager.controller.ChaseMovement.Run.canceled += StopRun;

        OnCollisionEnter(default);
    }

    void OnDisable()
    {
        InputManager.controller.ChaseMovement.Jump.performed -= Jump;
        InputManager.controller.ChaseMovement.Run.performed -= Run;
        InputManager.controller.ChaseMovement.Run.canceled -= StopRun;
        InputManager.controller.ChaseMovement.Disable();
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
        
        if (slopeNormal != Vector3.up)
        {
            slopeNormal = Vector3.up;
            rb.velocity = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
        }
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
        Vector3 finalVelocity = transform.TransformDirection(input);

        if (!grounded)
            finalVelocity *= airControl;
        else if (slopeNormal != Vector3.up && slopeNormal != Vector3.zero)
            finalVelocity = Vector3.ProjectOnPlane(finalVelocity, slopeNormal);

        ApplyForceToReachVelocity(finalVelocity, fForce);
    }

    // 2) Al entrar en colisión con algo, fijarse si es el piso o no. Eso determinará si puede saltar o no.
    void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return;

        grounded = CheckGround(checkOffsetEnter, halfExtentsEnter);

        CheckSlope();

        rb.drag = grounded ? groundDrag : airDrag;

        if (grounded && jumpBuffer > 0f)
            Jump(default);

        print(grounded + " " + slopeNormal);
    }

    public void OnCollisionExit(Collision collision)
    {
        if (!grounded || !enabled) return;

        grounded = CheckGround(checkOffsetExit, halfExtentsExit);

        rb.drag = grounded ? groundDrag : airDrag;

        CheckSlope();

        if (!grounded)
            coyoteBuffer = coyoteTime;

        print(grounded + " " + slopeNormal);
    }

    bool CheckGround(Vector3 offset, Vector3 halfExtents)
        => Physics.OverlapBoxNonAlloc(transform.position + offset, halfExtents, new Collider[16], Quaternion.identity, maskToIgnore) is not 0;

    void CheckSlope()
    {
        if (!grounded)
        {
            slopeNormal = Vector3.up;
            return;
        }

        Physics.Raycast(transform.position + rayCastOffset, Vector3.down, out var hitInfo, .25f, maskToIgnore);

        slopeNormal = hitInfo.normal;

        if (Vector3.Angle(Vector3.up, slopeNormal) >= maxSlopeAngle)
        {
            slopeNormal = Vector3.up;
            grounded = false;
        }
    }

    void ApplyForceToReachVelocity(Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force is 0f || velocity.magnitude is 0f)
            return;

        velocity += .2f * rb.drag * velocity.normalized;

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
