using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimelineInteractable : ActionInteractable
{
    [SerializeField] internal PlayableDirector timeline;
    [SerializeField] UnityEvent onTimelineEnd;
    [SerializeField] bool unsubscribeOnEnd = true;
    
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

    void TimelineEnd(PlayableDirector _)
    {
        if (unsubscribeOnEnd)
            timeline.stopped -= TimelineEnd;
        onTimelineEnd.Invoke();
    }
}
