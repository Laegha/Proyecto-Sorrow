using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseController : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;

    int trackedWayPoint = 0;

    /*[HideInInspector]*/ public bool isMoving;

    [SerializeField] float speed;

    float magnitude;
    float traveled;

    void Start() => TrackWaypoint();

    void Update()
    {
        if (!isMoving)
            return;

        var delta = speed * Time.deltaTime;
        transform.Translate(delta * Vector3.forward);
        traveled += delta;

        if (traveled < magnitude)
            return;

        trackedWayPoint++;
        if (trackedWayPoint > waypoints.Length - 1)
        {
            isMoving = false;
            return;
        }
        TrackWaypoint();
    }

    void TrackWaypoint()
    {
        var delta = new Vector2(waypoints[trackedWayPoint].position.x, waypoints[trackedWayPoint].position.z) - new Vector2(transform.position.x, transform.position.z);
        magnitude = delta.magnitude;
        traveled = 0f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg, 0f));
    }

    public void StartChase() => isMoving = true;
}