using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonMashing : MonoBehaviour
{
    public float mashCount;
    public float mashMin;
    [SerializeField] float disminutionMultiplier;
    [SerializeField] float mashIntencity;
    [SerializeField] Action action;

    void Awake() => mashCount = mashMin;

    void OnEnable()
    {
        InputManager.controller.ButtonMashing.Enable();
        InputManager.controller.ButtonMashing.Button.performed += Mash;
    }

    void OnDisable()
    {
        InputManager.controller.ButtonMashing.Button.performed -= Mash;
        InputManager.controller.ButtonMashing.Disable();
    }

    void Mash(InputAction.CallbackContext _)
    {
        mashCount -= mashIntencity;

        if (mashCount <= 0)
            End();
    }

    void Update()
    {
        mashCount += Time.deltaTime * disminutionMultiplier;
        Debug.Log(mashCount);
    }

    void End()
    {
        action.Invoke();
        InputManager.controller.ButtonMashing.Disable();
        enabled = false;
    }
}
