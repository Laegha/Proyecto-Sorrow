using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    Camera _camera;

    public float interactionDistance;
    void Start() => _camera = Camera.main;

    public void CheckInteraction(InputAction.CallbackContext context)
    {
        Physics.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, out RaycastHit hitObj, interactionDistance);
        if (hitObj.transform == null)
            return;
        foreach(Interactable interactable in hitObj.transform.GetComponents<Interactable>())
            interactable.Interaction();
    }
}