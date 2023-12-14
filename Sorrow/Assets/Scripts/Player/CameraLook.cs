using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLook : MonoBehaviour
{
    Animator PointerAnimator => GameObject.Find("Pointer").GetComponent<Animator>();
    Transform player;
    float CurrXRot => transform.localRotation.eulerAngles.x > 180f ? transform.localRotation.eulerAngles.x - 360f : transform.localRotation.eulerAngles.x;
    Rigidbody rb;

    private void Awake() => StartCoroutine(AssignCamera());

    IEnumerator AssignCamera()
    {
        yield return new WaitForEndOfFrame();
        CinematicManager.instance.playerCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = transform.root;
        rb = player.GetComponent<Rigidbody>();
    }

    void OnEnable() => PointerAnimator.SetBool("Show", true);
    void OnDisable()
    {
        if (gameObject.scene.isLoaded && PointerAnimator)
            PointerAnimator.SetBool("Show", false);
    }

    public void ChangeRotation(Quaternion rotation)
    {
        rb.MoveRotation(Quaternion.Euler(0f, rotation.eulerAngles.y, 0f));
        transform.localRotation = Quaternion.Euler(rotation.eulerAngles.x, 0f, 0f);
    }

    void Update()
    {
        var delta = InputManager.controller.Camera.Look.ReadValue<Vector2>();
        float mouseY = delta.y * InputManager.cameraSensitivity;
        float mouseX = delta.x * InputManager.cameraSensitivity;

        if (mouseX is not 0f)
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, mouseX, 0f));

        if (mouseY is 0f)
            return;

        transform.localRotation = Quaternion.Euler(Mathf.Clamp(CurrXRot - mouseY, -90f, 90f), 0f, 0f);
    }
}
