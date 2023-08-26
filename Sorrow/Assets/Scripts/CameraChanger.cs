using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    public void ChangeCamera(CinemachineVirtualCamera cam)
    {
        CinematicManager.CameraChange(cam);
    }
}
