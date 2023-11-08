using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpointer : MonoBehaviour
{
    public static int reachedCheckpoint = 0;

    [SerializeField] UnityEvent[] checkpointEvents;

    void Start() => checkpointEvents[reachedCheckpoint].Invoke();

    public static void ReachCheckpoint(int checkpointNumber) => reachedCheckpoint = checkpointNumber;
}
