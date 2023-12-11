using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyNeededDoorInteractable : HeldObjectNeedInteractable
{
    [SerializeField] Animator doorAnim;

    public override void Interaction()
    {
        base.Interaction();
        if (neededObjectHeld)
        {
            Destroy(heldObjectManager.heldObject.gameObject);
            doorAnim.Play("DoorOpen");
            GetComponent<AudioSource>().Play();
            Destroy(this);
        }
        else
            doorAnim.Play("CantOpen");
    }
}
