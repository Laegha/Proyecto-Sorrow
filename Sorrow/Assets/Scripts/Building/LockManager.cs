using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockManager : MonoBehaviour
{

    int selfPosition;
    float timer = 1.1f;
    float timeMultiplier, initRotation, finalRotation;

    void Awake() => selfPosition = name[^2] - 48;

    void OnEnable()
    {
        LockRhythmController.OnRotate += SetupRotation;
        LockRhythmController.OnUnlock += UnlockNums;
    }

    void OnDisable()
    {
        LockRhythmController.OnRotate -= SetupRotation;
        LockRhythmController.OnUnlock -= UnlockNums;
    }

    void Update()
    {
        if (timer >= 1.1f)
            return;

        timer += Time.deltaTime * timeMultiplier;
        var rotation = transform.eulerAngles;
        rotation.z = EaseInExpo(initRotation, finalRotation, timer);
        transform.eulerAngles = rotation;
    }

    void SetupRotation(object _, LockRhythmController.LockEventArgs e)
    {
        
        if (selfPosition <= e.CurrentBeat)
            return;

        timer = 0f;
        timeMultiplier = 1f / e.InTime;
        initRotation = transform.eulerAngles.z;
        finalRotation = LockRhythmController.CalcRotate(e.CurrentBeat);
    }

    void UnlockNums(object _, LockRhythmController.LockEventArgs e)
    {
        transform.eulerAngles = new(0f, 0f, LockRhythmController.CalcRotate(e.CurrentBeat));
        // Animate
    }

    public static float EaseInExpo(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }
}
