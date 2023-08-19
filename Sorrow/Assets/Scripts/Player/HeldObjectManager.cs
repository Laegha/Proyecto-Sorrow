using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectManager : MonoBehaviour
{
    Transform heldObjectHolder;

    [HideInInspector] public HeldObject heldObject;
    Collider heldObjectCollider;
    bool isHolding = false;

    void Start() => heldObjectHolder = transform.GetChild(0).Find("HeldObjectHolder");

    private void Update()
    {
        if (heldObject == null)
            return;

        if (!isHolding)
        {
            isHolding = true;
            return;
        }

        if (InputManager.controller.Player.UseItem.IsPressed() && heldObject.thisItem != null)
        {
            heldObject.thisItem.ItemEffect();
            if(heldObject.thisItem.isConsumable)
            {
                isHolding = false;
                heldObject = null;
            }
        }

        if (InputManager.controller.Player.Drop.IsPressed())
            DropObject();
    }

    public void HoldObject(HeldObject newHeldObject)
    {
        if (heldObject != null)
            return;

        heldObject = newHeldObject;

        heldObject.transform.SetParent(heldObjectHolder);

        heldObject.transform.localRotation= Quaternion.Euler(Vector3.zero);
        heldObject._rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        
        float objectLength = heldObject.meshRenderer.bounds.extents.z; //falta que se ajuste al tama�o
        heldObject.transform.localPosition = new Vector3(0.01f, 0.01f, objectLength);
        heldObjectCollider = heldObject.GetComponent<Collider>();
        heldObjectCollider.enabled = false; 
    }

    void DropObject()
    {
        heldObject.transform.SetParent(null);
        heldObject._rigidbody.constraints = RigidbodyConstraints.None;
        heldObjectCollider.enabled = true;
        heldObjectCollider = null;
        heldObject = null;
    }
}
