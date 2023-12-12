using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockRhythmController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera lockCamera;
    [SerializeField] float bpm, bpmIncrease, accuracyRange;
    [SerializeField] AudioClip[] audioClips;
    public static readonly int[,] finalPin = new int[4, 8];
    readonly int[] currentPin = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
    int lockPhase, lockedNums = 0;
    float beatDuration, accuracyDuration, currentBeatTimer;
    int currentBeat = 1;
    bool hasLocked, rotateEventSent = false;
    AudioSource audioSource;
    public static event System.EventHandler<LockEventArgs> OnRotate, OnUnlock;
    public static event System.EventHandler<int> OnPhase;

    void OnEnable()
    {
        CinematicManager.instance.CameraChange(lockCamera);
        InputManager.instance.RemRegControl(false);
        InputManager.controller.LockRythm.Enable();
        InputManager.controller.LockRythm.LockNum.performed += Lock;
        RecalculateHalfBeatDuration();
        audioSource.Play();
    }

    void OnDisable()
    {
        audioSource.Stop();
        CinematicManager.instance.ReturnPlayerCamera();
        InputManager.instance.RemRegControl(true);
        InputManager.controller.LockRythm.LockNum.performed -= Lock;
        InputManager.controller.LockRythm.Disable();
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 8; j++)
            {
                finalPin[i, j] = Random.Range(minInclusive: 1, maxExclusive: 5);
            }
    }

    void Update()
    {
        currentBeatTimer += Time.deltaTime;
        if (!rotateEventSent && currentBeatTimer >= accuracyDuration)
        {
            rotateEventSent = true;
            OnRotate?.Invoke(this, new LockEventArgs(lockedNums, beatDuration, currentBeat));
        }
        else if (currentBeatTimer < beatDuration) 
            return;
        
        currentBeat += currentBeat is not 4 ? 1 : -3;
        for (int i = 0; i < 8; i++)
            currentPin[i] = currentBeat;

        currentBeatTimer -= beatDuration;
        //OnRotate?.Invoke(this, new LockEventArgs(lockedNums, beatDuration, currentBeat));
    }

    void Lock(InputAction.CallbackContext _)
    {
        if (currentBeatTimer > accuracyDuration || hasLocked || !enabled)
            return;

        if (currentPin[lockedNums] == finalPin[lockPhase, lockedNums])
        {
            hasLocked = true;
            if (++lockedNums is 8)
            {
                bpm += bpmIncrease;
                RecalculateHalfBeatDuration();
                audioSource.clip = audioClips[++lockPhase];
                audioSource.time *= (bpm - bpmIncrease) / bpm;
                lockedNums = 0;
            }
        }
        else
        {
            lockedNums = 0;
            OnUnlock?.Invoke(this, new LockEventArgs(lockedNums, beatDuration, currentBeat));
        }

        if (lockPhase is not 4 && lockedNums is not 0)
            return;

        print("Unlocked");
        enabled = false;
    }

    void RecalculateHalfBeatDuration()
    {
        beatDuration = 60 / bpm;
        accuracyDuration = beatDuration * accuracyRange;
    }

    public static float CalcRotate(int beat) => beat switch
    {
        1 => 0f,
        2 => -90f,
        3 => -180f,
        4 => -270f,
        _ => 0f
    };

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
