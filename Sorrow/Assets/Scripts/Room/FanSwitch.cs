using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSwitch : HitableTarget
{
    Transform playerTransform;
    Rigidbody rb;
    CapsuleCollider boxCollider;
    PlayerMovement playerMovement;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        playerTransform = player.transform;
        rb = player.GetComponent<Rigidbody>();
        boxCollider = player.GetComponent<CapsuleCollider>();
        playerMovement = player.GetComponent<PlayerMovement>();

        rb.isKinematic = true;
        boxCollider.enabled = false;
        playerMovement.enabled = false;
    }

    public override void Activate()
    {
        //Iniciar timeline
        EndResult();
    }

    void EndResult() // DEBUG
    {
        playerTransform.Translate(Vector3.up);
        boxCollider.enabled = true;
        rb.isKinematic = false;
        playerMovement.enabled = true;
        enabled = false;
    }
}
