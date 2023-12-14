using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class KeyPadInteractable : ActionInteractable
{
    static int interactedTimes = 0;
    [SerializeField] GameObject[] depressionPositions;
    [SerializeField] CinemachineVirtualCamera depressionCamera;
    [SerializeField] Texture[] keypadTextures;
    [SerializeField] Renderer[] renderers;
    CameraLook cameraLook;
    KeypadRhythmController kprController;

    void Start()
    {
        var player = GameObject.Find("Player");
        cameraLook = player.GetComponentInChildren<CameraLook>();
        depressionCamera.transform.parent = player.transform;
        depressionCamera.transform.localPosition = CinematicManager.instance.playerCamera.transform.localPosition;
        kprController = FindObjectOfType<KeypadRhythmController>();
        GetComponent<Outline>().enabled = true;
    }

    void OnDisable()
    {
        if (!gameObject.scene.IsValid())
            return;

        Destroy(depressionCamera.gameObject);
        kprController.enabled = true;
    }

    public override void Interaction()
    {
        base.Interaction();
        if (interactedTimes == depressionPositions.Length - 1)
        {
            enabled = false;
            return;
        }

        depressionPositions[interactedTimes++].SetActive(false);
        depressionPositions[interactedTimes].SetActive(true);
        depressionCamera.LookAt = depressionPositions[interactedTimes].transform.GetChild(0).GetChild(0).Find("Face"); // Not proud of this either

        LookAtDepression();
    }

    void LookAtDepression()
    {
        CinematicManager.instance.CameraChange(depressionCamera);
        InputManager.instance.RemRegControl(false);
        Invoke(nameof(LockToDepression), 0.1f);
    }

    void LockToDepression()
    {
        foreach (var renderer in renderers)
            renderer.materials[0].SetTexture("_EmissionMap", keypadTextures[interactedTimes]);
        cameraLook.ChangeRotation(depressionCamera.transform.rotation);
        InputManager.instance.RemRegControl(true);
        CinematicManager.instance.ReturnPlayerCamera();
    }
}