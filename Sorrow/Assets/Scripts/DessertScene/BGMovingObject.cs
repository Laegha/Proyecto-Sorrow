using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMovingObject : MonoBehaviour
{
    [SerializeField] float objectSpeed;

    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    Vector2 direction => new Vector2(endPoint.position.x, endPoint.position.z) - new Vector2(startPoint.position.x, startPoint.position.z);
    void Update()
    {
        transform.Translate(direction * objectSpeed);

    }

}
