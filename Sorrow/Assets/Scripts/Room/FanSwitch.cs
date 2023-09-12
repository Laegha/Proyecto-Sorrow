using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSwitch : HitableTarget
{
    Transform playerTransform;
    Rigidbody rb;
    CapsuleCollider playerCollider;
    PlayerMovement playerMovement;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        playerTransform = player.transform;
        rb = player.GetComponent<Rigidbody>();
        playerCollider = player.GetComponent<CapsuleCollider>();
        playerMovement = player.GetComponent<PlayerMovement>();

    }

    public override void Activate()
    {
        //Iniciar timeline
        EndResult();
    }

    void EndResult() // DEBUG
    {
        playerTransform.Translate(Vector3.up);
        playerCollider.enabled = true;
        rb.isKinematic = false;
        playerMovement.enabled = true;
        enabled = false;
    }
}
