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
            doorAnim.Play("DoorOpen");
        else
            doorAnim.Play("CantOpen");
        Destroy(this);
    }
}
