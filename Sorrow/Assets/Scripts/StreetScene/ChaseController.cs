using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseController : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;

    int trackedWayPoint = 0;
    [SerializeField] float speed, turnSpeed;
    float distance, traveled, turned;
    Quaternion initRotation, finalRotation;
    Vector3 initPosition, finalPosition;
    bool isRotating;
    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Start() => TrackWaypoint();

    void Update()
    {
        if (isRotating)
        {
            rb.MoveRotation(Quaternion.Lerp(initRotation, finalRotation, turned));
            turned += turnSpeed * Time.deltaTime;
            if (turned >= 1f)
                isRotating = false;
            return;
        }

        /*
        // Delta method
        var delta = speed * Time.deltaTime;
        rb.MovePosition(transform.position + delta * transform.forward);
        traveled += delta;

        if (traveled < distance)
            return;
        */

        // Lerp method
        rb.MovePosition(Vector3.Lerp(initPosition, finalPosition, traveled));
        traveled += speed * Time.deltaTime / distance;

        if (traveled < 1f)
            return;

        if (++trackedWayPoint >= waypoints.Length)
        {
            enabled = false;
            return;
        }
        TrackWaypoint();
    }

    void TrackWaypoint()
    {
        print("Tracking waypoint " + trackedWayPoint);
        var horizontalWaypoint = new Vector2(waypoints[trackedWayPoint].position.x, waypoints[trackedWayPoint].position.z);
        var horizontalPosition = new Vector2(transform.position.x, transform.position.z);
        var delta = horizontalWaypoint - horizontalPosition;
        distance = Vector2.Distance(horizontalWaypoint, horizontalPosition);

        traveled = 0f;
        initPosition = transform.position;
        finalPosition = waypoints[trackedWayPoint].position;
        initRotation = transform.rotation;
        finalRotation = Quaternion.Euler(new Vector3(0f, Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg, 0f));
        turned = 0f;
        isRotating = true;
    }
    
    public void ChangeSpeed(float newSpeed) => speed = newSpeed;
}