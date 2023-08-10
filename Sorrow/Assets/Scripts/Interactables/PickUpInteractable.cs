using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteractable : Interactable
{
    [HideInInspector] public Rigidbody _rigidbody;
    [HideInInspector] public MeshRenderer meshRenderer;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public override void Interaction()
    {
        base.Interaction();
        FindObjectOfType<HeldObjectsManager>().HoldObject(this);
    }
}
