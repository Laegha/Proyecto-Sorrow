using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    InputManager inputManager;

    Camera _camera;

    public float interactionDistance;
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.interactionKey))
            CheckInteraction();
    }
    void CheckInteraction()
    {
        RaycastHit hitObj;
        Physics.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, out hitObj, 5);
        if (hitObj.transform == null)
            return;
        Interactable interactable = hitObj.transform.GetComponent<Interactable>();
        if (interactable != null)
            interactable.Interaction();
    }
}
