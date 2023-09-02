using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CinematicManager
{
    static CinemachineVirtualCamera currCamera;
    
    public static void CameraChange(CinemachineVirtualCamera newCamera)
    {
        if (currCamera != null)
            currCamera.Priority = 0;
        newCamera.Priority = 11;
        currCamera = newCamera;
    }

    public static void ReturnPlayerCamera()
    {
        currCamera.Priority = 0;
        currCamera = null;
    }
}
