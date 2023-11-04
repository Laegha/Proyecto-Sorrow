using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaphonePicaporte : HeldObjectNeedInteractable
{
    [SerializeField] GameObject megaphonePicaporte;
    public override void Interaction()
    {
        base.Interaction();
        if (!neededObjectHeld)
            return;
        heldObjectManager.heldObject.gameObject.SetActive(false);
        megaphonePicaporte.SetActive(true);
    }
}
