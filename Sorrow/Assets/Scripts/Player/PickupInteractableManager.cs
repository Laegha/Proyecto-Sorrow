using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractableManager : MonoBehaviour
{
    Transform heldObjectPosition;

    PickUpInteractable heldObject;

    Item heldObjectItem;
    void Start() => heldObjectPosition = transform.Find("HeldObjectPosition");
    public bool HoldObject(PickUpInteractable newHeldObject)
    {
        if (heldObject != null)
            return false;
        heldObject = newHeldObject;
        heldObjectItem = heldObject.thisItem;
        heldObject._rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        heldObject.transform.rotation = Quaternion.identity;
        Vector3 objectSize = heldObject.meshRenderer.bounds.size; //falta que se ajuste al tamaño
        heldObject.transform.position = heldObjectPosition.position;
        heldObject.transform.SetParent(Camera.main.transform);
        return true;
    }

}
