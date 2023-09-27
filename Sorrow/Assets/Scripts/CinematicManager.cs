using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager instance;

    GameObject player;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        player = GameObject.FindWithTag("Player");
    }

    CinemachineVirtualCamera currCamera;

    public void CameraChange(CinemachineVirtualCamera newCamera)
    {
        if (currCamera != null)
            currCamera.Priority = 0;
        newCamera.Priority = 11;
        currCamera = newCamera;
    }

    public void ReturnPlayerCamera()
    {
        currCamera.Priority = 0;
        currCamera = FindObjectOfType<CameraLook>().GetComponent<CinemachineVirtualCamera>();
    }

    public void PlayerFreeze(bool isFreezed)
    {
        Camera.main.transform.SetParent(isFreezed ? null : player.transform);
        player.SetActive(!isFreezed);
        //Camera.main.GetComponent<CameraLook>().enabled = !isFreezed;
    }

    public void CameraShake(float newShakeScaleDecrease)
    {
        startPosition = currCamera.transform.localPosition;
        shakeScaleDecrease = newShakeScaleDecrease;
    }

    bool shaking = false;
    float shakeScaleDecrease;
    Vector3 startPosition;

    private void Update()
    {
        if(shaking)
        {
            currCamera.transform.localPosition = startPosition + Random.insideUnitSphere / shakeScaleDecrease;

        }
        
    }
}
