using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePickUpInteractable : ActionInteractable
{
    [SerializeField] GameObject pickUpHolder;
    HeldObjectManager heldObjectManager;
    HeldObject[] pickups;
    int lastPickedUp = 0;

    protected override void Awake()
    {
        base.Awake(); 
    
        heldObjectManager = FindObjectOfType<HeldObjectManager>();
        pickups = pickUpHolder.GetComponentsInChildren<HeldObject>();
    }

    public override void Interaction()
    {
        base.Interaction();

        if (heldObjectManager.heldObject != null)
            return;
        heldObjectManager.HoldObject(pickups[lastPickedUp]);
        lastPickedUp++;
        if(lastPickedUp == pickups.Length)
            lastPickedUp = 0;
    }
}
