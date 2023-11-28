using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FakeDoorInteractable : Interactable
{
    [SerializeField] ChaseController chaseController;
    [SerializeField] GameObject hands1;
    [SerializeField] GameObject hands2;
    PickUpInteractable pickUpInteractable;
    ButtonMashing buttonMashing;

    [SerializeField] PlayableDirector timeline;

    protected override void Awake()
    {
        base.Awake();

        pickUpInteractable = GetComponent<PickUpInteractable>();
        pickUpInteractable.enabled = false;
        buttonMashing = GameObject.FindGameObjectWithTag("Player").GetComponent<ButtonMashing>();
    }

    void Start() {
        enabled = false;
        enabled = true;
    }

    public override void Interaction()
    {
        base.Interaction();

        enabled = false;
        Checkpointer.ReachCheckpoint(1);
        //gameObject.layer = LayerMask.NameToLayer("PP");

        StartCoroutine(StartChaseCinematic());
    }

    IEnumerator StartChaseCinematic()
    {
        //CinematicManager.instance.PlayerFreeze(true);
        timeline.Play();
        CinematicManager.instance.CameraChange(buttonMashing.buttonMashingVCam);
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
        //CinematicManager.instance.PlayerFreeze(false);
        //InputManager.instance.GetComponent<PlayerMovement>().enabled = false;
        //InputManager.instance.GetComponent<PlayerChaseMovement>().enabled = true;

        //InputManager.instance.GetComponent<HeldObjectManager>().HoldObject(heldObject);
        //megaphoneComponent.enabled = true;
        //enabled = false;
        chaseController.enabled = true;
        Destroy(hands1);
        hands2.SetActive(true);
        buttonMashing.enabled = true;
        Destroy(this);
    }
}