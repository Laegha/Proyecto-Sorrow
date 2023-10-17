using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DessertTimelineResources : MonoBehaviour
{
    [SerializeField] float movingSandMaxSpeed;
    [SerializeField] Renderer sandRenderer;
    Material sandMaterial => sandRenderer.material;

    [SerializeField] BGMovingObject[] bgObjects;

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
        float a = movingSandMaxSpeed/1;
        if (!isStarting)
            a *= -1;

        while(isStarting ? y < movingSandMaxSpeed : y > 0)
        {
            yield return new WaitForEndOfFrame();
            
            x += Time.deltaTime;
            float y = a * (x * x) + oao;
            print("X: " + x + " y: " + y + " a: " + a);
            sandMaterial.SetFloat("_Speed", y);
        }
        sandMaterial.SetFloat("_Speed", isStarting ? movingSandMaxSpeed : 0);
    }

    bool sandMoving = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            sandMoving = !sandMoving;
            ChangeWalkMode(sandMoving);
        }
    }
}
