using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FanSwitch : HitableTarget
{
    [SerializeField] PlayableDirector timeline;

    public override void Activate()
    {
        timeline.Play();
        enabled = false;
        Destroy(this);
    }
}
