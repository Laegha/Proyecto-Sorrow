using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChaseMovement : MonoBehaviour
{
    [SerializeField] float lerpFactor;
    [SerializeField] float moveDrag;
    [SerializeField] float stopDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCheckOffset;
    [SerializeField] float jumpBufferTime;
    Rigidbody rb;
    Vector2 input;
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
        input = context.ReadValue<Vector2>();
        rb.drag = moveDrag;
    }

    public void StopRun(InputAction.CallbackContext context)
    {
        input = Vector2.zero;
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

        if (input == Vector2.zero)
        {
            t = 0f;
            return;
        }

        if (t < 1f)
            t += lerpFactor * Time.deltaTime;
        else
            t = 1f;

        var vector = Mathf.Lerp(0f, 1f, t) * input;

        //if (rb.velocity.magnitude < maxBurstSpeed)
            //vector *= burstAccelMult;

        //rb.AddRelativeForce(vector.x , 0, vector.y, ForceMode.Force);

        rb.velocity = transform.TransformDirection(new Vector3(vector.x, rb.velocity.y, vector.y));
    }

    // 2) Al entrar en colisión con algo, fijarse si es el piso o no. Eso determinará si puede saltar o no.
    void OnCollisionEnter(Collision collision)
    {
        if (!enabled) return;

        canJump = CheckGround();

        if (!canJump) return;

        if (jumpBuffer > 0f)
            Jump(default);
        
        if (input == Vector2.zero)
            rb.drag = stopDrag;
    }

    bool CheckGround()
        => Physics.OverlapBox(transform.position + new Vector3(0f, jumpCheckOffset, 0f), halfExtents).Length is not 0;
}
