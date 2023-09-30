using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FanSwitch : HitableTarget
{
    public override void Activate()
    {
        GetComponent<PlayableDirector>().Play();
        enabled = false;
        Destroy(this);
    }
}
