using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectNeedInteractable : Interactable
{
    HeldObjectManager heldObjectManager;

    [SerializeField] string neededObjectName;

    public bool neededObjectHeld;
    protected override void Awake()
    {
        base.Awake();

        heldObjectManager = FindObjectOfType<HeldObjectManager>();
    }

    public override void Interaction()
    {
        if (heldObjectManager.heldObject == null)
            return;

        if (heldObjectManager.heldObject.objectName != neededObjectName)
            return;

        neededObjectHeld = true;
    }
}