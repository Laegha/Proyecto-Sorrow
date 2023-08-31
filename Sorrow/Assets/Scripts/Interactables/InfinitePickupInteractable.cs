using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePickUpInteractable : Interactable
{
    [SerializeField] GameObject pickUpPrefab;

    HeldObjectManager heldObjectManager;

    protected override void Start()
    {
        base.Start(); 
    
        heldObjectManager = FindObjectOfType<HeldObjectManager>();
    }

    public override void Interaction()
    {
        GameObject pickUp = Instantiate(pickUpPrefab, Vector3.zero, Quaternion.identity);
        heldObjectManager.HoldObject(pickUp.GetComponent<HeldObject>());
    }
}
