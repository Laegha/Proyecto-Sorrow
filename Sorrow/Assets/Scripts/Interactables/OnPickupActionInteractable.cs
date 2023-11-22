using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPickupActionInteractable : PickUpInteractable
{
    [SerializeField] UnityEvent onInteractEvent;

    public override void Interaction()
    {
        base.Interaction();
        onInteractEvent.Invoke();
    }
}
