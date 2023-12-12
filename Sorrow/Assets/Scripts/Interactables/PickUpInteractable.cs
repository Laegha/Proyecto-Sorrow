using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteractable : Interactable
{
    public override void Interaction()
    {
        if (!enabled)
            return;

        enabled = false;
        FindObjectOfType<HeldObjectManager>().HoldObject(GetComponent<HeldObject>());

        HeldObjectNeedInteractable[] locks = FindObjectsOfType<HeldObjectNeedInteractable>();
        foreach (HeldObjectNeedInteractable _lock in locks)
            if (_lock.neededObjectName == GetComponent<HeldObject>().objectName)
                _lock.ChangeOutlineHoverColor(Color.white);
    }
}
