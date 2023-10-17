using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager instance;
    [HideInInspector]public CinemachineBrain cinemachineBrain;
    CinemachineVirtualCamera CurrCamera => cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
    CinemachineVirtualCamera currCamera;
    [HideInInspector] public CinemachineVirtualCamera playerCamera;
    [HideInInspector] public GameObject player;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        player = GameObject.FindWithTag("Player");
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        playerCamera = GameObject.Find("PlayerVirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    public void CameraChange(CinemachineVirtualCamera newCamera)
    {
        if (currCamera != null)
            currCamera.Priority = 0;
        else
            print("Current camera was null, now is " + newCamera.name);

        newCamera.Priority = 11;
        SetNewCamera(newCamera);
    }

    public void SetNewCamera(CinemachineVirtualCamera newCamera) => currCamera = newCamera;

    public void ReturnPlayerCamera()
    {
        print("Player cam returned");
        currCamera.Priority = 0;
        playerCamera.gameObject.SetActive(true);
        playerCamera.Priority= 11;
        currCamera = playerCamera;
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
        CurrCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;

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
