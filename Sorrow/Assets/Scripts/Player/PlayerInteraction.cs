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
        Physics.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, out RaycastHit hitObj, 5);
        if (hitObj.transform == null)
            return;
        if (hitObj.transform.TryGetComponent<Interactable>(out var interactable))
            interactable.Interaction();
    }
}
