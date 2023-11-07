using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionInteractable : Interactable
{
    [SerializeField] UnityEvent onInteractEvent;

    public override void Interaction() => onInteractEvent.Invoke();

    public void ChangeCamera(CinemachineVirtualCamera cam) => CinematicManager.instance.CameraChange(cam);
}
