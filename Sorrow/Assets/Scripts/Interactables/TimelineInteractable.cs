using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimelineInteractable : ActionInteractable
{
    [SerializeField] PlayableDirector timeline;
    [SerializeField] UnityEvent onTimelineEnd;
    
    protected override void Awake()
    {
        base.Awake();
        timeline.stopped += TimelineEnd;
    }

    public override void Interaction()
    {
        base.Interaction();
        timeline.Play();
    }

    void TimelineEnd(PlayableDirector _) => onTimelineEnd.Invoke();
}
