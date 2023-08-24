using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChaseMovement : MonoBehaviour
{
    [SerializeField] bool useForceMode;
    [SerializeField] float fForce;
    [SerializeField] float lerpFactor;
    [SerializeField] float minLerp;
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
    float t;
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
        var vector2 = context.ReadValue<Vector2>();
        input = new(vector2.x, 0f, vector2.y);
        rb.drag = moveDrag;
    }

    public void StopRun(InputAction.CallbackContext context)
    {
        input = Vector3.zero;
        if (CheckGround())
            rb.drag = stopDrag;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("SHOOT");
    }

    void Update()
    {
        //Debug.Log("V " + rb.velocity.magnitude + " " + rb.GetPointVelocity(transform.position));

        jumpBuffer -= Time.deltaTime;
        
        if (Mathf.Abs(rb.velocity.y) > .5f)
            rb.drag = moveDrag;

        if (useForceMode)
            return;

        if (input == Vector3.zero)
        {
            t = 0f;
            return;
        }

        if (t < 1f)
            t += lerpFactor * Time.deltaTime;
        else
            t = 1f;
        
        var vector = Mathf.Lerp(minLerp, 1f, t) * input;
        vector.y = rb.velocity.y;
        rb.velocity = transform.TransformDirection(vector);
    }

    void FixedUpdate()
    {
        if (!useForceMode)
            return;

        Vector3 finalVelocity = transform.TransformDirection(input);

        bool isGoingOpositeZ = Mathf.Abs(rb.velocity.z) > 0.1f && Mathf.Sign(finalVelocity.z) != Mathf.Sign(rb.velocity.z);
        bool airborne = rb.velocity.y != 0f && !CheckGround();

        if (airborne && isGoingOpositeZ)
            if (rb.velocity.y < -.1f)
                finalVelocity *= -airControl;
            else
                return;

        ApplyForceToReachVelocity(finalVelocity, fForce);

        /* 
        Ok, me quiero ir a acostar, el tema es así:
        vos aca si dejas de apretar en el aire va a frenar también, vos no queres eso, vos queres que frene pero no tanto como si apretase para atras
        también, no tiene que ser apretar para "atras" tiene que ser el contrario de la velocidad actualmente
        debe de ser algo tan facil como pasar el input a world y despues un == entre los 2 Math.Sign

        atte y buenas atrasadas noches
        - el que te quiere (sugerencia de github copilot)
        */

        /*
        if (Mathf.Abs(rb.velocity.y) < .5f || input.z > 0f)
        {
            ApplyForceToReachVelocity(transform.TransformDirection(input), fForce);
            return;
        }

        // Airborne and not pushing forward

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);

        Vector3 slowdownVector = new(
            localVelocity.x/2,
            0f,
            localVelocity.z / (input.z == 0f ? 2f : Mathf.Abs(input.z))
        );

        ApplyForceToReachVelocity(transform.TransformDirection(slowdownVector), fForce);
        */
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
