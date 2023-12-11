using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    int selfPosition;

    void Awake() => selfPosition = name[^2] - 48;

    void OnEnable()
    {
        LockRhythmController.OnRotate += Rotate;
        LockRhythmController.OnUnlock += UnlockNums;
    }

    void OnDisable()
    {
        LockRhythmController.OnRotate -= Rotate;
        LockRhythmController.OnUnlock -= UnlockNums;
    }

    void Rotate(object _, LockRhythmController.LockEventArgs e)
    {
        if (e.CurrentBeat >= selfPosition)
            return;
            
        transform.eulerAngles = new(0f, 0f, CalcRotate(e.CurrentBeat));
        // Activate the animation
    }

    void UnlockNums(object _, LockRhythmController.LockEventArgs e)
    {
        transform.eulerAngles = new(0f, 0f, CalcRotate(e.CurrentBeat));
        // Animate
    }

    float CalcRotate(int beat) => beat switch
    {
        1 => 0f,
        2 => -90f,
        3 => -180f,
        4 => -270f,
        _ => 0f
    };
}
