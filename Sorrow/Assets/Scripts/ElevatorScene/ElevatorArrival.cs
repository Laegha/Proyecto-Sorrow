using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorArrival : MonoBehaviour
{
    [SerializeField] PlayableDirector endTimeline;
    [SerializeField] Vector3 newElevatorPosition;
    [SerializeField] Transform elevator;

    public void StartArrival()
    {
        GameObject.FindWithTag("Player").transform.SetParent(elevator);
        elevator.position = newElevatorPosition;
        GameObject.FindWithTag("Player").transform.SetParent(null);

        endTimeline.Play();
    }
}
