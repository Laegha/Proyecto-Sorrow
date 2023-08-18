//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChaseController : MonoBehaviour
//{
//    Transform[] waypoints;

//    int trackedWayPoint = 0;

//    [SerializeField] float speed;

//    void Chase()
//    {
//        Vector2 direction = new Vector2 (waypoints[trackedWayPoint].position.x, waypoints[trackedWayPoint].position.z) - new Vector2 (transform.position.x, transform.position.z);
//        transform.Translate(new Vector3(direction.x, 0, direction.y) * speed);
//        if(ReachedWaypoint())
//    }
//    bool ReachedWaypoint()
//    {

//        if()
//    }
//}
