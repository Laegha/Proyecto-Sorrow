using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FanSwitch : HitableTarget
{
    [SerializeField] PlayableDirector director;
    [SerializeField] PlayableAsset timeline;

    public override void Activate()
    {
        director.playableAsset = timeline;
        director.Play();
        enabled = false;
        Destroy(this);
    }
}
