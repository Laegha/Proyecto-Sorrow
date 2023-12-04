using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetNPCOpacity : MonoBehaviour
{
    [SerializeField] float maxDistance = 40f;
    [SerializeField] float minDistance = 10f;
    [SerializeField] MeshRenderer handR;
    [SerializeField] MeshRenderer handL;
    Material materialBody;
    Vector3 lastPosition;
    Color fullColor;

    void Start()
    {
        materialBody = GetComponent<MeshRenderer>().material;
        fullColor = materialBody.GetColor("_Color");
    }

    void Update()
    {
        if (Camera.main.transform.position == lastPosition)
            return;
        
        lastPosition = Camera.main.transform.position;
        float distance = Mathf.Abs(transform.position.x - lastPosition.x);
        if (distance > maxDistance)
        {
            materialBody.SetColor("_Color", new Color(fullColor.r, fullColor.g, fullColor.b, 0f));
            handR.material.SetColor("_Color", new Color(fullColor.r, fullColor.g, fullColor.b, 0f));
            handL.material.SetColor("_Color", new Color(fullColor.r, fullColor.g, fullColor.b, 0f));
            return;
        } else if (distance < minDistance)
        {
            materialBody.SetColor("_Color", fullColor);
            handR.material.SetColor("_Color", fullColor);
            handL.material.SetColor("_Color", fullColor);
            return;
        }

        float opacity = 1f - (distance - minDistance) / (maxDistance - minDistance);
        materialBody.SetColor("_Color", new Color(fullColor.r, fullColor.g, fullColor.b, opacity));
        handR.material.SetColor("_Color", new Color(fullColor.r, fullColor.g, fullColor.b, opacity));
        handL.material.SetColor("_Color", new Color(fullColor.r, fullColor.g, fullColor.b, opacity));
    }
}
