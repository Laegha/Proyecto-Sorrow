using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CodePaperInteractable : OnPickupActionInteractable
{
    [SerializeField] Transform targetToLook;
    [SerializeField] float playerRotationSpeed;
    [SerializeField] float cameraRotationSpeed;

    public void PlayerRotate() => StartCoroutine(RotatePlayer());

    IEnumerator RotatePlayer()
    {
        InputManager.instance.RemRegControl(false);
        
        StartCoroutine(RotateCamera());

        Transform player = CinematicManager.instance.player.transform;
        Vector2 delta = new Vector2(targetToLook.position.x, targetToLook.position.z) - new Vector2(player.position.x, player.position.z);

        float rotationAngle = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;

        player.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;

        yield return new WaitForEndOfFrame();

        while (player.rotation.eulerAngles.y > rotationAngle)
        {
            player.Rotate(0, Time.deltaTime * -playerRotationSpeed, 0);
            yield return new WaitForEndOfFrame();
        }

        player.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        InputManager.instance.RemRegControl(true);
    }

    IEnumerator RotateCamera()
    {
        Transform camera = CinematicManager.instance.playerCamera.transform;
        float counter = camera.localRotation.eulerAngles.x;
        while (counter > 0.5f)
        {
            counter -= Time.deltaTime * cameraRotationSpeed;
            camera.Rotate(Time.deltaTime * -cameraRotationSpeed, 0, 0);
            yield return new WaitForEndOfFrame();
        }
    }
}