using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorArrival : MonoBehaviour
{
    [SerializeField] PlayableDirector endTimeline;
    [SerializeField] Vector3 newElevatorPosition;

    public void StartArrival()
    {
        GameObject.FindWithTag("Player").transform.SetParent(transform);
        transform.SetPositionAndRotation(newElevatorPosition, Quaternion.Euler(0, 180, 0));
        GameObject.FindWithTag("Player").transform.SetParent(null);

        endTimeline.Play();
    }
}
