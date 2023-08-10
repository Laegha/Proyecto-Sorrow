using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectsManager : MonoBehaviour
{
    Transform heldObjectPosition;

    PickUpInteractable heldObject;
    void Start()
    {
        heldObjectPosition = transform.Find("HeldObjectPosition");
    }
    public bool HoldObject(PickUpInteractable newHeldObject)
    {
        if (heldObject != null)
            return false;
        heldObject = newHeldObject;
        heldObject._rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Vector3 objectSize = heldObject.meshRenderer.bounds.size;
        heldObject.transform.position = heldObjectPosition.position;
        heldObject.transform.SetParent(transform);
        return true;
    }

}
