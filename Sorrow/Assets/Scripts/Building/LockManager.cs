using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    [SerializeField] Color lockedColor, wrongColor;
    [SerializeField] AudioClip lockSound, unlockSound, phaseSound;
    Color unlockedColor;
    int selfPosition;
    float timer = 1.1f;
    float timeMultiplier, initRotation, finalRotation;
    float baseRotation = 0f;
    Material material;
    AudioSource audioSource;

    void Awake()
    {
        selfPosition = name[^2] - 48;
        baseRotation = transform.eulerAngles.z;
        material = GetComponent<MeshRenderer>().material;
        unlockedColor = material.color;
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        LockRhythmController.OnRotate += SetupRotation;
        LockRhythmController.OnUnlock += UnlockNum;
        LockRhythmController.OnLock += LockNum;
        LockRhythmController.OnPhase += Phase;
    }

    void OnDisable()
    {
        LockRhythmController.OnRotate -= SetupRotation;
        LockRhythmController.OnUnlock -= UnlockNum;
        LockRhythmController.OnLock -= LockNum;
        LockRhythmController.OnPhase -= Phase;
    }

    void Update()
    {
        if (timer >= 1.1f)
            return;

        timer += Time.deltaTime * timeMultiplier;
        var rotated = transform.eulerAngles;
        rotated.z = EaseInExpo(initRotation, finalRotation, timer);
        transform.eulerAngles = rotated;
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
        if (material.color == lockedColor || e.LockedNums + 1 == selfPosition)
            material.color = wrongColor;

        audioSource.PlayOneShot(unlockSound);
        timer = 0f;
        timeMultiplier = 1f / e.InTime;
        initRotation = transform.eulerAngles.z;
        finalRotation = baseRotation + LockRhythmController.CalcRotate(e.CurrentBeat + 1);
    }

    void LockNum(object _, LockRhythmController.LockEventArgs e)
    {
        if (e.LockedNums != selfPosition)
            return;
        
        material.color = lockedColor;
        timer = 1.1f;
        var rotated = transform.eulerAngles;
        rotated.z = baseRotation + LockRhythmController.CalcRotate(e.CurrentBeat);
        transform.eulerAngles = rotated;
        audioSource.PlayOneShot(lockSound);
    }

    void Phase(object _, LockRhythmController.LockEventArgs e)
    {
        audioSource.PlayOneShot(phaseSound);
        timer = 0f;
        timeMultiplier = 1f / e.InTime;
        initRotation = transform.eulerAngles.z;
        finalRotation = baseRotation + LockRhythmController.CalcRotate(e.CurrentBeat + 1);
    }

    public static float EaseInExpo(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);

        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }
}
