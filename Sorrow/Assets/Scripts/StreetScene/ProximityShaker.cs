using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ProximityShaker : MonoBehaviour
{
    [SerializeField] Transform distanceTo;
    [SerializeField] float distanceMultiplier = 1f;
    [SerializeField] float minDistance = 60f;
    Vector3 lastSelfPos;
    Vector3 lastToPos;

    void OnDisable() => CinematicManager.instance.StopCameraShake();

    void Update()
    {
        if (transform.position == lastSelfPos && distanceTo.position == lastToPos)
            return;
        
        lastToPos = distanceTo.position;
        lastSelfPos = transform.position;

        var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(distanceTo.position.x, distanceTo.position.z));

        if (distance > minDistance)
            CinematicManager.instance.StopCameraShake();
        else
            CinematicManager.instance.StartCameraShake(distanceMultiplier * (minDistance - distance));
    }
}
