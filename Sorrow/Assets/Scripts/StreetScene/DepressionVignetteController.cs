using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepressionVignetteController : MonoBehaviour
{
    [SerializeField] Transform distanceFrom;
    [SerializeField] Transform distanceTo;
    [SerializeField] float distanceMultiplier = 1f;
    [SerializeField] float minFresnelPower = 1f;
    Material material;
    Vector3 lastFromPos;
    Vector3 lastToPos;

    void Awake() => material = GetComponent<Image>().material;

    void Update()
    {
        if (distanceFrom.position == lastFromPos && distanceTo.position == lastToPos)
            return;
        
        lastToPos = distanceTo.position;
        lastFromPos = distanceFrom.position;
        material.SetFloat("_FresnelPower", minFresnelPower + distanceMultiplier * Vector2.Distance(new Vector2(distanceFrom.position.x, distanceFrom.position.z), new Vector2(distanceTo.position.x, distanceTo.position.z)));
    }
}
