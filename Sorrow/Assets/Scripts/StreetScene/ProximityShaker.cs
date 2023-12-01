using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ProximityShaker : MonoBehaviour
{
    [SerializeField] Transform distanceTo;
    [SerializeField] float minDistance = 60f;
    [SerializeField] float maxShake = 2f;
    float distanceMultiplier;
    Vector3 lastSelfPos;
    Vector3 lastToPos;

    void Awake() => RecalculateDistanceMultiplier();

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
            CinematicManager.instance.StartCameraShake(distanceMultiplier * Mathf.Pow(minDistance - distance, 2));
    }

    public void ChangeMinDistance(float newMinDistance)
    {
        minDistance = newMinDistance;
        RecalculateDistanceMultiplier();
    }

    public void ChangeMaxShake(float newMaxShake)
    {
        maxShake = newMaxShake;
        RecalculateDistanceMultiplier();
    }

    void RecalculateDistanceMultiplier() => distanceMultiplier = maxShake / Mathf.Pow(minDistance - 10f, 2);
}
