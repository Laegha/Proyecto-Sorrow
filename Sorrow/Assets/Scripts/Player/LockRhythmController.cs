using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockRhythmController : MonoBehaviour
{
    [SerializeField] float bpm;
    [SerializeField] float bpmIncrease;
    [SerializeField] float accuracyRange;
    public static readonly int[,] finalPin = new int[4, 8];
    readonly int[] currentPin = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
    int lockPhase = 0;
    int lockedNums = 0;
    float lockBeatDuration;
    float rotateBeatDuration;
    int currentBeat = 1;
    bool hasLocked = false;
    bool canLock = true;
    public static event System.EventHandler<LockEventArgs> OnRotate;
    public static event System.EventHandler<LockEventArgs> OnUnlock;

    void OnEnable()
    {
        InputManager.controller.LockRythm.Enable();
        InputManager.controller.LockRythm.LockNum.performed += Lock;
        RecalculateHalfBeatDuration();
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
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 8; j++)
            {
                finalPin[i, j] = Random.Range(minInclusive: 1, maxExclusive: 5);
            }
    }

    IEnumerator MetronomeCoroutine()
    {
        while (enabled)
        {
            for (int i = lockedNums; i < 8; i++)
                currentPin[i] = currentBeat;
            print(string.Join("", currentPin) + "\n" + string.Join("", finalPin));
            yield return new WaitForSeconds(lockBeatDuration);
            canLock = false;
            print("rotating\n" + string.Join("", finalPin));
            yield return new WaitForSeconds(rotateBeatDuration);
            currentBeat += currentBeat != 4 ? 1 : -3;
            hasLocked = false;
            canLock = true;
            OnRotate?.Invoke(this, new LockEventArgs(lockedNums, lockBeatDuration, currentBeat));
        }
    }

    void Lock(InputAction.CallbackContext _)
    {
        if (!canLock || hasLocked || !enabled)
            return;

        if (currentPin[lockedNums] == finalPin[lockPhase, lockedNums])
        {
            hasLocked = true;
            if (++lockedNums is 8)
            {
                bpm += bpmIncrease;
                RecalculateHalfBeatDuration();
                lockPhase++;
                lockedNums = 0;
            }
        }
        else
        {
            lockedNums = 0;
            OnUnlock?.Invoke(this, new LockEventArgs(lockedNums, lockBeatDuration, currentBeat));
        }

        if (lockPhase is not 4 && lockedNums is not 0)
            return;

        StopCoroutine(MetronomeCoroutine());
        print("Unlocked");
    }

    void RecalculateHalfBeatDuration()
    {
        lockBeatDuration = 60 / bpm * accuracyRange;
        rotateBeatDuration = 60 / bpm * (1 - accuracyRange);
    }

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
