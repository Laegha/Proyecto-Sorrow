using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionInteractable : Interactable
{
    [SerializeField] UnityEvent onInteractEvent;

    protected override void Awake()
    {
        base.Awake();

        FindObjectOfType<ActionCounter>().neededActions ++;
    }

    public override void Interaction()
    {
        onInteractEvent.Invoke();
        FindObjectOfType<ActionCounter>().ActionDone();
    }

    public void ChangeCamera(CinemachineVirtualCamera cam)
    {
        CinematicManager.instance.CameraChange(cam);
    }
}
