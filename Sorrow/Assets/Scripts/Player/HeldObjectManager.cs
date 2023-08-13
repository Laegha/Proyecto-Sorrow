using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectManager : MonoBehaviour
{
    Transform heldObjectPosition;

    HeldObject heldObject;

    Item heldObjectItem;
    void Start() => heldObjectPosition = transform.Find("HeldObjectPosition");

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.inputManager.useItemKey) && heldObjectItem != null)
            if (heldObjectItem.ItemEffect())
                heldObjectItem = null;
        if (Input.GetKeyDown(InputManager.inputManager.dropObjectKey))
            if (heldObject != null)
                DropObject();
    }
    public void HoldObject(HeldObject newHeldObject)
    {
        if (heldObject != null)
            return;
        heldObject = newHeldObject;
        heldObjectItem = heldObject.thisItem;

        heldObject.transform.SetParent(Camera.main.transform);

        heldObject.transform.localRotation= Quaternion.Euler(Vector3.zero);
        heldObject._rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        
        Vector3 objectSize = heldObject.meshRenderer.bounds.size; //falta que se ajuste al tamaño
        heldObject.transform.position = heldObjectPosition.position + Vector3.forward * objectSize.z;
    }

    void DropObject()
    {
        heldObject.transform.SetParent(null);
        heldObject._rigidbody.constraints = RigidbodyConstraints.None;
        heldObject = null;
    }
}
