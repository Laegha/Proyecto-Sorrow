using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMovingObject : MonoBehaviour
{
    [SerializeField] float objectSpeed;

    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    Vector2 direction;
    float distance;
    float elapsedDistance;

    private void Start()
    {
        var delta = new Vector2(endPoint.position.x, endPoint.position.z) - new Vector2(startPoint.position.x, startPoint.position.z);
        direction = delta.normalized;
        distance = delta.magnitude;

    }

    void Update()
    {
        var delta = objectSpeed * Time.deltaTime;
        transform.Translate(new Vector3(direction.x, 0, direction.y) * delta);
        elapsedDistance += delta;

        if (elapsedDistance > distance)
            ResetPosition();
    }

    void ResetPosition()
    {
        transform.position = startPoint.position;
        elapsedDistance = 0;
    }
}
