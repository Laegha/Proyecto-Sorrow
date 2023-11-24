using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertTimelineResources : MonoBehaviour
{
    [SerializeField] float movingSandMaxSpeed;
    [SerializeField] Renderer sandRenderer;
    Material SandMaterial => sandRenderer.material;
    [SerializeField] Vector2 sandMoveDirection;

    [SerializeField] BGMovingObject[] bgObjects;

    bool sandMoving = false;
    Vector2 currOffset;
    float speed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeWalkMode(!sandMoving);
        }//PLACEHOLDER

        currOffset += speed * Time.deltaTime * sandMoveDirection;
        if (currOffset.magnitude > 1)
            currOffset = Vector2.zero;
        SandMaterial.SetVector("_TextureOffset", currOffset);
    }
    public void ChangeWalkMode(bool isStarting)
    {
        StartCoroutine(ChangeMovingSandSpeed(isStarting));
        foreach (BGMovingObject bgObject in bgObjects)
            bgObject.isMoving = isStarting;
    }

    public IEnumerator ChangeMovingSandSpeed(bool isStarting)
    {
        float x = 0;
        float oao = isStarting ? 0 : movingSandMaxSpeed; 
        float a = movingSandMaxSpeed/2;

        if (!isStarting)
            a *= -1;

        while (isStarting ? speed < movingSandMaxSpeed : speed > 0)
        {
            yield return new WaitForEndOfFrame();

            x += Time.deltaTime;
            speed = a * (x * x) + oao;
        }
        speed = isStarting ? movingSandMaxSpeed : 0;
        yield return new WaitForEndOfFrame();
        sandMoving = isStarting;
    }

    public void MovePlayer(Transform copiedPosition) => CinematicManager.instance.player.transform.position = new Vector3(copiedPosition.position.x, CinematicManager.instance.player.transform.position.y, copiedPosition.position.z);
}
