using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    [SerializeField] Color lockedColor, wrongColor;
    Color unlockedColor;
    int selfPosition;
    float timer = 1.1f;
    float timeMultiplier, initRotation, finalRotation;
    float baseRotation = 0f;
    Material material;

    void Awake()
    {
        selfPosition = name[^2] - 48;
        baseRotation = transform.eulerAngles.z;
        material = GetComponent<MeshRenderer>().material;
        unlockedColor = material.color;
    }

    void OnEnable()
    {
        LockRhythmController.OnRotate += SetupRotation;
        LockRhythmController.OnUnlock += UnlockNum;
        LockRhythmController.OnLock += LockNum;
    }

    void OnDisable()
    {
        LockRhythmController.OnRotate -= SetupRotation;
        LockRhythmController.OnUnlock -= UnlockNum;
        LockRhythmController.OnLock -= LockNum;
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

        material.color = unlockedColor;
        timer = 0f;
        timeMultiplier = 1f / e.InTime;
        initRotation = transform.eulerAngles.z;
        finalRotation = initRotation - 90f;
    }

    void UnlockNum(object _, LockRhythmController.LockEventArgs e)
    {
        if (material.color == lockedColor)
            material.color = wrongColor;

        var rotated = transform.eulerAngles;
        rotated.z = baseRotation + LockRhythmController.CalcRotate(e.CurrentBeat);
        transform.eulerAngles = rotated;
        // Animate
    }

    void LockNum(object _, int lockedNums)
    {
        if (lockedNums < selfPosition)
            return;
        
        material.color = lockedColor;
    }

    public static float EaseInExpo(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);

        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }
}
