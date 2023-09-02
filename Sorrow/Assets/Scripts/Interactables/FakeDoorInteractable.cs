using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDoorInteractable : Interactable
{
    [SerializeField] GameObject debugWall;
    [SerializeField] ChaseController chaseController;
    Megaphone megaphoneComponent;
    HeldObject heldObject;
    PickUpInteractable pickUpInteractable;
    int timesInteracted = 0;

    void Awake()
    {
        megaphoneComponent = GetComponent<Megaphone>();
        heldObject = GetComponent<HeldObject>();
        pickUpInteractable = GetComponent<PickUpInteractable>();
    }

    public override void Interaction()
    {
        base.Interaction();
        if (timesInteracted < 2)
        {
            timesInteracted++;
            print($"Interacted {timesInteracted}");
            if (timesInteracted == 2)
                pickUpInteractable.enabled = true;
            // Trying to open timeline
            return;
        }

        InputManager.instance.GetComponent<PlayerMovement>().enabled = false;

        /*
        TIMELINE HERE:

        1. Player turns around and sees the moster coming
        2. Player turns back around and grabs the door handle
        3. Controller map changes to button mashing
        4. Player breaks the handle getting the megaphone
        5. Wall falls down
        6. Player regains controll

        */

        // Final result
        InputManager.instance.GetComponent<PlayerChaseMovement>().enabled = true;
        Destroy(debugWall);
        InputManager.instance.GetComponent<HeldObjectManager>().HoldObject(heldObject);
        print("RUN");
        megaphoneComponent.enabled = true;
        chaseController.isMoving = true;
        enabled = false;
    }
}