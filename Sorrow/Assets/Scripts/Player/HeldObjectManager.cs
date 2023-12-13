using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeldObjectManager : MonoBehaviour
{
    Transform heldObjectHolder;
    [HideInInspector] public HeldObject heldObject;
    Collider heldObjectCollider;

    void Awake() => heldObjectHolder = transform.GetChild(0).Find("HeldObjectHolder");

    void OnEnable() => StartCoroutine(SubscribeEvents());

    IEnumerator SubscribeEvents()
    {
        yield return new WaitForEndOfFrame();

        InputManager.controller.Camera.Click.performed += UseObject;
        InputManager.controller.Camera.Click.performed += CheckInteraction;
    }
    
    void OnDisable()
    {
        InputManager.controller.Camera.Click.performed -= UseObject;
        InputManager.controller.Camera.Click.performed -= CheckInteraction;
    }

    void CheckInteraction(InputAction.CallbackContext context)
    {
        Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, out RaycastHit hitObj);
        if (hitObj.transform == null)
            return;
        foreach(Interactable interactable in hitObj.transform.GetComponents<Interactable>().Where(x => x.enabled))
            if(interactable.interactionDistance > hitObj.distance)
                interactable.Interaction();
    }

    public void HoldObject(HeldObject newHeldObject)
    {
        if (heldObject != null)
            return;

        heldObject = newHeldObject;

        heldObject.transform.SetParent(heldObjectHolder);

        heldObject.transform.localRotation = Quaternion.identity;
        
        float objectLength = heldObject.meshRenderer.bounds.extents.z; 
        heldObject.transform.localPosition = new Vector3(0.01f, 0.01f, objectLength);
        heldObjectCollider = heldObject.GetComponent<Collider>();
        heldObjectCollider.enabled = false;

        heldObject._rigidbody.isKinematic = true;
        heldObject._rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
    }

    public void UseObject(InputAction.CallbackContext _)
    {

        if (heldObject == null || heldObject.thisItem == null)
            return;

        heldObject.thisItem.ItemEffect();
        if (!heldObject.thisItem.isConsumable)
            return;

        StartCoroutine(ClearHeldObject());
        heldObjectCollider.enabled = true;
    }

    IEnumerator ClearHeldObject()
    {
        yield return new WaitForEndOfFrame();
        heldObject = null;
    }

    public void ReleaseObject()
    {
        if (heldObject == null || heldObject.thisItem == null)
            return;

        heldObject.transform.SetParent(null);
        heldObject._rigidbody.constraints = RigidbodyConstraints.None;
        heldObjectCollider.enabled = true;
        heldObjectCollider = null;
        heldObject = null;
    }
}
