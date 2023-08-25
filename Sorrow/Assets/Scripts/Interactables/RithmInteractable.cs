using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RithmInteractable : Interactable
{
    public int empanadas;
    [SerializeField] CinemachineVirtualCameraBase botoneraCamera;    
    public override void Interaction()
    {
        botoneraCamera.Priority = 10;
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
    }
}
