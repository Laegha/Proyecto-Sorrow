using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteractable : Interactable
{
    public override void Interaction() => FindObjectOfType<HeldObjectManager>().HoldObject(GetComponent<HeldObject>());
    
}
