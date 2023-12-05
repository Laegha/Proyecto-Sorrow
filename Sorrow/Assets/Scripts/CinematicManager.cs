using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager instance;
    [HideInInspector]public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera CurrCamera => cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
    [HideInInspector] public CinemachineVirtualCamera playerCamera;
    [HideInInspector] public GameObject player;

    private void Awake()
    {
        if (instance != null)
        {
            playerCamera = instance.playerCamera;
            Destroy(instance);
        }
        instance = this;
        player = GameObject.FindWithTag("Player");
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }

    public void CameraChange(CinemachineVirtualCamera newCamera)
    {
        if (CurrCamera != null)
            CurrCamera.enabled = false;

        newCamera.enabled = true;
    }

    public void ReturnPlayerCamera()
    {
        print("Player cam returned");
        CurrCamera.enabled = false;
        playerCamera.gameObject.SetActive(true);
        playerCamera.enabled = true;
    }

    public void StartCameraShake(float shakeAmplitude)
    {
        CurrCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeAmplitude;

        /*
        startPosition = CurrCamera.transform.localPosition;
        shaking = true;
        shakeScaleDecrease = newShakeScaleDecrease;
        */
    }

    public void StopCameraShake()
    {
        if (!CurrCamera)
            return;
        var noiseChannel = CurrCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noiseChannel)
            noiseChannel.m_AmplitudeGain = 0f;

        /*
        shaking = false;
        CurrCamera.transform.localPosition = startPosition;
        */
    }
    /*
    bool shaking = false;
    float shakeScaleDecrease;
    Vector3 startPosition;

    void Update()
    {
        if(shaking)
        {
            CurrCamera.transform.localPosition = startPosition + Random.insideUnitSphere / shakeScaleDecrease;

        }
        
    }
    */
}
