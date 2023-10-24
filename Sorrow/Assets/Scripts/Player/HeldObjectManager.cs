using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeldObjectManager : MonoBehaviour
{
    Camera _camera;
    public float interactionDistance;
    Transform heldObjectHolder;
    [HideInInspector] public HeldObject heldObject;
    Collider heldObjectCollider;

    void Start()
    {
        heldObjectHolder = transform.GetChild(0).Find("HeldObjectHolder");
        _camera = Camera.main;
    }

    void OnEnable() => StartCoroutine(SubscribeEvents());
    IEnumerator SubscribeEvents()
    {
        yield return new WaitForEndOfFrame();

        InputManager.controller.Camera.Click.performed += UseObject;
        InputManager.controller.Camera.Drop.performed += DropObject;
        InputManager.controller.Camera.Click.performed += CheckInteraction;
    }
    void OnDisable()
    {
        InputManager.controller.Camera.Click.performed -= UseObject;
        InputManager.controller.Camera.Drop.performed -= DropObject;
        InputManager.controller.Camera.Click.performed -= CheckInteraction;
    }

    void CheckInteraction(InputAction.CallbackContext context)
    {
        Physics.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, out RaycastHit hitObj, interactionDistance);
        if (hitObj.transform == null)
            return;
        foreach(Interactable interactable in hitObj.transform.GetComponents<Interactable>().Where(x => x.enabled))
            interactable.Interaction();
    }

    public void HoldObject(HeldObject newHeldObject)
    {
        if (heldObject != null)
            return;

        heldObject = newHeldObject;

        heldObject.transform.SetParent(heldObjectHolder);

        heldObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        heldObject._rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        
        float objectLength = heldObject.meshRenderer.bounds.extents.z; //falta que se ajuste al tama�o
        heldObject.transform.localPosition = new Vector3(0.01f, 0.01f, objectLength);
        heldObjectCollider = heldObject.GetComponent<Collider>();
        heldObjectCollider.enabled = false;
    }

    public void UseObject(InputAction.CallbackContext _)
    {

        if (heldObject == null || heldObject.thisItem == null)
            return;

        heldObject.thisItem.ItemEffect();
        if (!heldObject.thisItem.isConsumable)
            return;

        heldObject = null;
        heldObjectCollider.enabled = true;
    }

    void DropObject(InputAction.CallbackContext _)
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
