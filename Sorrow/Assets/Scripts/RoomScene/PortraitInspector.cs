using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PortraitInspector : Interactable
{
    [SerializeField] CinemachineVirtualCamera portraitCamera;
    [SerializeField] Transform lookingTransform;
    [SerializeField] float transitionSpeed = 1f;
    [SerializeField] Vector2 rotationCaps;
    Transform originalTransform;
    bool isLooking = false;
    float transitionTime = 1f;
    Vector2 currRot, maxRot, minRot;

    protected override void Awake()
    {
        base.Awake();
        originalTransform = typeof(object).GetMethod("MemberwiseClone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(transform, Array.Empty<object>()) as Transform;
        minRot = maxRot = lookingTransform.eulerAngles;
        minRot -= rotationCaps;
        maxRot += rotationCaps;
    }

    public override void Interaction()
    {
        isLooking ^= true;
        transitionTime = 0f;
        currRot = lookingTransform.eulerAngles;
        if (!isLooking)
            CinematicManager.instance.playerCamera.transform.rotation = Quaternion.identity;
        CinematicManager.instance.CameraChange(isLooking ? portraitCamera : CinematicManager.instance.playerCamera);
    }

    void Update()
    {
        if (transitionTime < 1f)
        {
            transitionTime += Time.deltaTime * transitionSpeed;
            transform.SetPositionAndRotation(
                Vector3.Lerp(originalTransform.position, lookingTransform.position, transitionTime),
                Quaternion.Lerp(originalTransform.rotation, lookingTransform.rotation, transitionTime)
            );
            return;
        } else if (!isLooking) return;

        var delta = InputManager.controller.Camera.Look.ReadValue<Vector2>();
        float mouseY = delta.y * InputManager.cameraSensitivity;
        float mouseX = delta.x * InputManager.cameraSensitivity;

        if (mouseX is not 0f)
            currRot.y = Mathf.Clamp(currRot.y - mouseX, minRot.y, maxRot.y);

        if (mouseY is not 0f)
            currRot.x = Mathf.Clamp(currRot.x - mouseY, minRot.x, maxRot.x);

        if (mouseX is not 0f || mouseY is not 0f)
            transform.rotation = Quaternion.Euler(currRot);
    }

}
