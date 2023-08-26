using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionInteractable : Interactable
{
    [SerializeField] UnityEvent onInteractEvent;

    protected override void Start()
    {
        base.Start();

        FindObjectOfType<ActionCounter>().neededActions ++;
    }

    public override void Interaction()
    {
        onInteractEvent.Invoke();
    }

    public void ChangeCamera(CinemachineVirtualCamera cam)
    {

    }
}
