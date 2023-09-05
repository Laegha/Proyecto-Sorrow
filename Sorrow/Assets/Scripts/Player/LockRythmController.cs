using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockRythmController : MonoBehaviour
{
    const int totalNums = 32;
    [SerializeField] float bpm;
    [SerializeField] float bpmIncrease;
    readonly int[] finalPin = new int[totalNums];
    readonly int[] currentPin = new int[totalNums];
    int lockedNums = 0;
    float halfBeatDuration;
    int currentBeat = 1;
    bool hasLocked = false;
    bool canLock = true;
    public static event System.EventHandler<LockEventArgs> OnRotate;
    public static event System.EventHandler<LockEventArgs> OnUnlock;

    void OnEnable()
    {
        InputManager.controller.LockRythm.Enable();
        InputManager.controller.LockRythm.LockNum.performed += Lock;
        StartCoroutine(MetronomeCoroutine());
    }

    void OnDisable()
    {
        InputManager.controller.LockRythm.LockNum.performed -= Lock;
        InputManager.controller.LockRythm.Disable();
        StopCoroutine(MetronomeCoroutine());
    }

    void Awake()
    {
        for (int i = 0; i < totalNums; i++)
        {
            finalPin[i] = Random.Range(minInclusive: 1, maxExclusive: 5);
            currentPin[i] = 1;
        }
    }

    IEnumerator MetronomeCoroutine()
    {
        while (enabled)
        {
            for (int i = lockedNums; i < totalNums; i++)
                currentPin[i] = currentBeat;
            print(string.Join("", currentPin) + "\n" + string.Join("", finalPin));
            yield return new WaitForSeconds(halfBeatDuration);
            canLock = false;
            print("rotating");
            OnRotate.Invoke(this, new LockEventArgs(lockedNums, halfBeatDuration, currentBeat));
            yield return new WaitForSeconds(halfBeatDuration);
            currentBeat += currentBeat != 4 ? 1 : -3;
            hasLocked = false;
            canLock = true;
        }
    }

    void Lock(InputAction.CallbackContext _)
    {
        if (!canLock || hasLocked || !enabled)
            return;

        if (currentPin[lockedNums] == finalPin[lockedNums])
        {
            lockedNums++;
            hasLocked = true;
            if (lockedNums % 8 is 0)
            {
                bpm += bpmIncrease;
                RecalculateHalfBeatDuration();
            }
        }
        else
        {
            lockedNums -= lockedNums % 8;
            OnUnlock.Invoke(this, new LockEventArgs(lockedNums, halfBeatDuration, currentBeat));
        }

        if (lockedNums is not totalNums)
            return;

        StopCoroutine(MetronomeCoroutine());
        print("Unlocked");
    }

    void RecalculateHalfBeatDuration() => halfBeatDuration = 60 / bpm / 2;

    public class LockEventArgs : System.EventArgs
    {
        public int LockedNums { get; }
        public float InTime { get; }
        public int CurrentBeat { get; }

        public LockEventArgs(int lockedNums, float inTime, int currentBeat)
        {
            LockedNums = lockedNums;
            InTime = inTime;
            CurrentBeat = currentBeat;
        }
    }
}
