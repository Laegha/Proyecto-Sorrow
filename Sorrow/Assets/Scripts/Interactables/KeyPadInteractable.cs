using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadInteractable : RithmInteractable
{
    KeypadButton[] buttons => GetComponentsInChildren<KeypadButton>();

    protected override void Start()
    {
        base.Start();

        foreach(KeypadButton button in buttons) 
            button.keyPadInteractable = this;
    }

    public override void StartMinigame()
    {
        //Cursor.lockState = CursorLockMode.Confined;

    }
}
