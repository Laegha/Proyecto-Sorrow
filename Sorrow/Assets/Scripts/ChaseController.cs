//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChaseController : MonoBehaviour
//{
//    Transform[] waypoints;

//    int trackedWayPoint = 0;

//    [HideInInspector] public bool isMoving;

//    [SerializeField] float speed;

//    Vector2 delta;
//    float direction;

//    private void Start() => TrackWaypoint();

//    private void Update()
//    {
//        if (!isMoving)
//            return;

//        transform.Translate(new Vector3(delta.x, 0, delta.y) * speed * Time.deltaTime);
//        if (delta.)
//        {
//            trackedWayPoint++;
//            if (trackedWayPoint > waypoints.Length)
//            {
//                isMoving = false;
//                return;
//            }
//            TrackWaypoint();
//        }
//    }

//    void TrackWaypoint()
//    {
//        delta = new Vector2(waypoints[trackedWayPoint].position.x, waypoints[trackedWayPoint].position.z) - new Vector2(transform.position.x, transform.position.z);
//        direction = Mathf.
//    }
//}