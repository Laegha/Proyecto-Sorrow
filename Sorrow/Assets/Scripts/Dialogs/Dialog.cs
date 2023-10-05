using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public int Count => dialogTable.Count;
    public Color npcColor;
    [SerializeField] List<TimelineInterrupt> interrupts;
    [SerializeField] StringTable dialogTable;

    public string GetLine(int line)
        => dialogTable.GetEntry(line).GetLocalizedString();

    public bool TryInterrupt(int line, out PlayableAsset timeline, out bool dontStopText)
    {
        dontStopText = false;
        timeline = null;
        if (interrupts.Count is 0)
            return false;

        var firstInterrupt = interrupts.First();
        if (firstInterrupt.atLine != line)
            return false;
        
        timeline = firstInterrupt.timeline;
        dontStopText = firstInterrupt.dontStopText;
        interrupts.RemoveAt(0);
        return true;
    }
}
