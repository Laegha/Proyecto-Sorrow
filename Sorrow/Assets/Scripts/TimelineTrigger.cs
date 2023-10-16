using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    [SerializeField] PlayableDirector timelineDirector;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            timelineDirector.Play();
    }
}
