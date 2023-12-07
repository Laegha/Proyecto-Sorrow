using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    [Header("Dialog")]
    [SerializeField] LocalizedStringTable dialogTables;
    StringTable dialogTable;
    public Color npcColor;

    [Header("Timelines")]
    public PlayableAsset preTimeline;
    [SerializeField] List<TimelineInterrupt> interrupts;
    public PlayableAsset postTimeline;
    int currIndex = 0;

    public int Count => dialogTable.Count;

    public void SetLanguage()
        => dialogTable = dialogTables.GetTable();

    public string GetLine(int line, out bool isPlayer)
    {
        var entry = dialogTable.GetEntry($"{line}") ?? dialogTable.GetEntry($"-{line}");
        isPlayer = entry.Key.StartsWith('-');
        return entry.GetLocalizedString();
    }

    public bool TryInterrupt(int line, out PlayableAsset timeline, out bool dontStopText)
    {
        dontStopText = false;
        timeline = null;
        if (interrupts.Count is 0)
            return false;

        var currInterrupt = interrupts[currIndex];
        if (currInterrupt.atLine != line)
            return false;

        currIndex++;
        timeline = currInterrupt.timeline;
        dontStopText = currInterrupt.dontStopText;
        return true;
    }
}
