using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class ButtonMashing : MonoBehaviour
{
    [HideInInspector] public float mashCount;
    public float mashMin;
    [SerializeField] float timeToLoose1MashProgress;
    [SerializeField] float fovIntencity;
    [SerializeField] PlayableDirector director;
    [SerializeField] PlayableAsset timeline;
    public CinemachineVirtualCamera buttonMashingVCam;
    [SerializeField] UnityEvent endActions;
    float minFov;
    [SerializeField] ParticleSystem slamParticles;
    [SerializeField] float particleDelay;

    void Awake() => mashCount = mashMin;

    void OnEnable()
    {
        minFov = buttonMashingVCam.m_Lens.FieldOfView;
        InputManager.controller.ButtonMashing.Enable();
        InputManager.controller.ButtonMashing.Button.performed += Mash;
        director.playableAsset = timeline;
        director.time = director.duration;
        director.Evaluate();
    }

    void OnDisable()
    {
        InputManager.controller.ButtonMashing.Button.performed -= Mash;
        InputManager.controller.ButtonMashing.Disable();
    }

    void Mash(InputAction.CallbackContext _)
    {
        mashCount -= 1f;
        buttonMashingVCam.m_Lens.FieldOfView += fovIntencity;
        director.Stop();
        if (mashCount <= 0f)
           StartCoroutine(End());

        director.Play();
    }

    void Update()
    {
        if (mashCount < mashMin)
            mashCount += Time.deltaTime / timeToLoose1MashProgress;
        if (mashCount > mashMin)
            mashCount = mashMin;
        if (buttonMashingVCam.m_Lens.FieldOfView > minFov)
            buttonMashingVCam.m_Lens.FieldOfView -= Time.deltaTime / timeToLoose1MashProgress * fovIntencity;
        if (buttonMashingVCam.m_Lens.FieldOfView < minFov)
            buttonMashingVCam.m_Lens.FieldOfView = minFov;
    }

    IEnumerator End()
    {
        enabled = false;
        director.Stop();
        CinematicManager.instance.ReturnPlayerCamera();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(CinematicManager.instance.cinemachineBrain.ActiveBlend.Duration);

        endActions.Invoke();

        yield return new WaitForSeconds(particleDelay);
        slamParticles.Play();
    }
}
