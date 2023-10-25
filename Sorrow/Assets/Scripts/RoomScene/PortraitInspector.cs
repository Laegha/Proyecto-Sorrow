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
    Vector3 originalPosition;
    Quaternion originalRotation;
    bool isLooking = false;
    float transitionTime = 1f;
    Vector2 currRot, maxRot, minRot;
    CameraLook cameraLook;

    protected override void Awake()
    {
        base.Awake();
        //originalTransform = typeof(object).GetMethod("MemberwiseClone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(transform, Array.Empty<object>()) as Transform;
        originalPosition = transform.position; // Sometimes dark magic is fun
        originalRotation = transform.rotation; // But sometimes being greedy is better
        minRot = maxRot = lookingTransform.eulerAngles;
        minRot -= rotationCaps;
        maxRot += rotationCaps;
        cameraLook = FindObjectOfType<CameraLook>();
    }

    public override void Interaction()
    {
        isLooking ^= true;
        transitionTime = isLooking ? 0f : 1f;
        cameraLook.enabled = !isLooking;
        if (isLooking)
            InputManager.controller.Camera.Enable();
        else
            lookingTransform.rotation = transform.rotation;
        currRot = lookingTransform.eulerAngles;
        CinematicManager.instance.CameraChange(isLooking ? portraitCamera : CinematicManager.instance.playerCamera);
    }

    void Update()
    {
        if (0f <= transitionTime && transitionTime <= 1f)
        {
            transitionTime += Time.deltaTime * transitionSpeed * (isLooking ? 1f : -1f);
            transform.SetPositionAndRotation(
                Vector3.Lerp(originalPosition, lookingTransform.position, transitionTime),
                Quaternion.Lerp(originalRotation, lookingTransform.rotation, transitionTime)
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
