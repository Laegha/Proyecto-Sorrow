using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [HideInInspector] public KeyPadInteractable keyPadInteractable;

    bool waitingForBeat;

    private void OnMouseDown()
    {
        if (!waitingForBeat)
            return;
        //keyPadInteractable
        waitingForBeat = false;
    }
}
