using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockManager : MonoBehaviour
{

    int selfPosition;
    float timer = 1.1f;
    float timeMultiplier, initRotation, finalRotation;
    float accRotation = 0f;

    void Awake()
    {
        selfPosition = name[^2] - 48;
        accRotation = transform.eulerAngles.z;
    }

    void OnEnable()
    {
        LockRhythmController.OnRotate += SetupRotation;
        LockRhythmController.OnUnlock += UnlockNum;
    }

    void OnDisable()
    {
        LockRhythmController.OnRotate -= SetupRotation;
        LockRhythmController.OnUnlock -= UnlockNum;
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
        
        if (selfPosition <= e.LockedNums)
            return;

        timer = 0f;
        timeMultiplier = 1f / e.InTime;
        initRotation = transform.eulerAngles.z;
        finalRotation = initRotation - 90f; //LockRhythmController.CalcRotate(e.CurrentBeat) + accRotation;
        //print($"Init: {initRotation}, Final: {finalRotation}");
    }

    void UnlockNum(object _, LockRhythmController.LockEventArgs e)
    {
        accRotation = 0f;
        var rotated = transform.eulerAngles;
        rotated.z = accRotation + LockRhythmController.CalcRotate(e.CurrentBeat);
        transform.eulerAngles = rotated;
        // Animate
    }

    public static float EaseInExpo(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);

        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }
}
