using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [HideInInspector] public KeyPadInteractable keyPadInteractable;

    bool waitingForBeat;

    private void OnMouseDown()
    {
        print("Clickeaste " + name);
        if (!waitingForBeat)
            return;
        //keyPadInteractable
        waitingForBeat = false;
    }
}
