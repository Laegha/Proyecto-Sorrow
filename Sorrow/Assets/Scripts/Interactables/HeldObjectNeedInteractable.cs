using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectNeedInteractable : Interactable
{
    HeldObjectManager heldObjectManager;

    [SerializeField] string neededObjectName;
    private void Start()
    {
        heldObjectManager = FindObjectOfType<HeldObjectManager>();
    }
    public override void Interaction()
    {
        if (heldObjectManager.heldObject != null)
            if (heldObjectManager.heldObject.objectName == neededObjectName)
                print("Se interactúa efectivamente con " + transform.name);
    }
}
