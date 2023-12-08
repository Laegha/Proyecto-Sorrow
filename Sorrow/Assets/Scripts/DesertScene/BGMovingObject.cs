using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMovingObject : MonoBehaviour
{
    [SerializeField] float objectSpeed;
    [SerializeField] float chanceOfSpawningPerFrame;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    Vector2 direction;
    float distance;
    float elapsedDistance;

    public bool isMoving = false;
    bool isDespawned = false;

    private void Start()
    {
        var delta = new Vector2(endPoint.position.x, endPoint.position.z) - new Vector2(startPoint.position.x, startPoint.position.z);
        direction = delta.normalized;
        distance = delta.magnitude;
    }

    void Update()
    {
        if (!isMoving)
            return;
        var delta = objectSpeed * Time.deltaTime;
        transform.Translate(new Vector3(direction.x, 0, direction.y) * delta);
        elapsedDistance += delta;

        if (elapsedDistance > distance)
            isDespawned = true;

        if (isDespawned && Random.Range(0f, 1f) < chanceOfSpawningPerFrame)
            ResetPosition();
    }

    void ResetPosition()
    {
        isDespawned = false;
        transform.position = startPoint.position;
        elapsedDistance = 0;
    }
}
