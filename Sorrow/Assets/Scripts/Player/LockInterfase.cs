using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockInterfase : MonoBehaviour
{
    public Action lockAction;
    public Action startAction;


    public void Lock(InputAction.CallbackContext _)
    {
        if (enabled)
            lockAction?.Invoke();
    }

    public void StartLock(InputAction.CallbackContext _)
    {
        if (enabled)
            startAction?.Invoke();
    }
}
