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

    public void Mash(InputAction.CallbackContext _)
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
