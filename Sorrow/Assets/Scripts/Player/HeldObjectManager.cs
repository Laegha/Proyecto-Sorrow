using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectManager : MonoBehaviour
{
    Transform heldObjectPosition;

    [HideInInspector] public HeldObject heldObject;

    bool isHolding = false;
    void Start() => heldObjectPosition = transform.Find("HeldObjectPosition");

    private void Update()
    {
        if (heldObject == null)
            return;
        if(!isHolding)
        {
            isHolding= true;
            return;
        }
        if (Input.GetKeyDown(InputManager.inputManager.useItemKey) && heldObject.thisItem != null)
        {
            heldObject.thisItem.ItemEffect();
            if(heldObject.thisItem.isConsumable)
            {
                isHolding = false;
                heldObject = null;
            }
        }
        if (Input.GetKeyDown(InputManager.inputManager.dropObjectKey))
                DropObject();
    }
    public void HoldObject(HeldObject newHeldObject)
    {
        if (heldObject != null)
            return;

        heldObject = newHeldObject;

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
