using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineUtils : MonoBehaviour
{
    PlayableDirector playableDirector;

    void Awake() => playableDirector = GetComponent<PlayableDirector>();

    public void SkipTo(float time) => playableDirector.time = time;
}
