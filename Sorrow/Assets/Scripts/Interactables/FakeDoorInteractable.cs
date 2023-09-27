using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FakeDoorInteractable : Interactable
{
    [SerializeField] GameObject debugWall;
    [SerializeField] ChaseController chaseController;
    Megaphone megaphoneComponent;
    HeldObject heldObject;
    PickUpInteractable pickUpInteractable;
    int timesInteracted = 0;

    [SerializeField] PlayableDirector timeline;

    protected override void Awake()
    {
        base.Awake();

        megaphoneComponent = GetComponent<Megaphone>();
        heldObject = GetComponent<HeldObject>();
        pickUpInteractable = GetComponent<PickUpInteractable>();
        pickUpInteractable.enabled = false;
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

        StartCoroutine(StartChaseCinematic());
    }

    IEnumerator StartChaseCinematic()
    {
        CinematicManager.instance.PlayerFreeze(true);
        timeline.Play();

        yield return new WaitForSeconds((float)timeline.duration);
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
        CinematicManager.instance.PlayerFreeze(false);
        InputManager.instance.GetComponent<PlayerMovement>().enabled = false;
        InputManager.instance.GetComponent<PlayerChaseMovement>().enabled = true;

        InputManager.instance.GetComponent<HeldObjectManager>().HoldObject(heldObject);
        megaphoneComponent.enabled = true;
        chaseController.isMoving = true;
        enabled = false;
    }
}