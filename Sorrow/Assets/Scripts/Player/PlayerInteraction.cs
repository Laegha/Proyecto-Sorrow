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
        inputManager = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inputManager.interactionKey))
            CheckInteraction();
    }
    void CheckInteraction()
    {
        RaycastHit hitObj;
        Physics.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), _camera.transform.forward, out hitObj, 5);
        Interactable interactable = hitObj.transform.GetComponent<Interactable>();
        if (interactable != null)
            interactable.Interaction();
    }
}
