using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DessertTimelineResources : MonoBehaviour
{
    [SerializeField] float movingSandMaxSpeed;
    [SerializeField] Renderer sandRenderer;
    Material sandMaterial => sandRenderer.material;
    [SerializeField] Vector2 sandMoveDirection;

    [SerializeField] BGMovingObject[] bgObjects;

    public void ChangeWalkMode(bool isStarting)
    {
        StartCoroutine(ChangeMovingSandSpeed(isStarting));
        foreach (BGMovingObject bgObject in bgObjects)
            bgObject.isMoving = isStarting;
    }

    bool sandMoving = false;
    Vector2 currOffset;
    float speed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeWalkMode(!sandMoving);
        }//PLACEHOLDER

        currOffset = sandMoveDirection * speed * time;
        sandMaterial.SetVector("_TextureOffset", currOffset);
    }

    float time = 1;

    IEnumerator Timer()
    {
        print("Empieza el timer");
        time = 0;
        while(true)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            if (time > 1/speed)
            {
                print("Time reset with time: " + time);
                time = 0;
            }
        }
    }

    public IEnumerator ChangeMovingSandSpeed(bool isStarting)
    {
        //sandMaterial.SetFloat("_AutoMove", 0);
        float x = 0;
        float oao = isStarting ? 0 : movingSandMaxSpeed;
        float a = movingSandMaxSpeed/2;

        if (!isStarting)
            a *= -1;

        while (isStarting ? speed < movingSandMaxSpeed : speed > 0)
        {
            yield return new WaitForEndOfFrame();

            //sandMaterial.SetFloat("_Speed", y);
            x += Time.deltaTime;
            speed = a * (x * x) + oao;
        }
        speed = isStarting ? movingSandMaxSpeed : 0;
        yield return new WaitForEndOfFrame();
        sandMoving = isStarting;

        if (isStarting)
            StartCoroutine("Timer");
        
        else
        {
            StopCoroutine("Timer");
            time = 1;
        }
        //sandMaterial.SetFloat("_Speed", isStarting ? movingSandMaxSpeed : 0);
        //sandMaterial.SetFloat("_AutoMove", 1);
    }


}
