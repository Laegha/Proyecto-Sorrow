using System;
using UnityEngine.Playables;

[Serializable]
public struct TimelineInterrupt
{
    public int atLine;
    public PlayableAsset timeline;
    public bool dontStopText;
}
