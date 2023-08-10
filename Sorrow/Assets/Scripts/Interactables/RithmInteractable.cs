using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RithmInteractable : Interactable
{
    public override void Interaction()
    {
        base.Interaction();

        print("Interacted with " + transform.name);
    }
}
