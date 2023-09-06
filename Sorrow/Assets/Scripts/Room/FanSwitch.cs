using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSwitch : HitableTarget
{
    Transform playerTransform;
    Rigidbody rb;
    BoxCollider boxCollider;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        playerTransform = player.transform;
        rb = player.GetComponent<Rigidbody>();
        boxCollider = player.GetComponent<BoxCollider>();


        rb.isKinematic = true;
        boxCollider.enabled = false;

        InputManager.controller.Player.Disable();
    }

    public override void Activate()
    {
        //Iniciar timeline
        
    }

    void EndResult() // DEBUG
    {
        transform.Translate(Vector3.up);
        rb.isKinematic = false;
        boxCollider.enabled = true;
    }
}
