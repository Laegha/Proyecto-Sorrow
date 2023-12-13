using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class LockRhythmController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera lockCamera;
    [SerializeField] float bpm, bpmIncrease;
    [SerializeField] bool continuousLockingFeature = false;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] UnityEvent completionActions;
    public static readonly int[,] finalPin = new int[3, 8];
    readonly int[] currentPin = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
    int lockPhase, lockedNums = 0;
    float beatDuration, stillDuration, tfsBeatDuration, currentBeatTimer;
    int currentBeat = 1;
    bool hasLocked, eventSent = false;
    AudioSource audioSource;
    public static event System.EventHandler<LockEventArgs> OnRotate, OnUnlock, OnPhase, OnLock;
    LockEventArgs CurrentLockEventArgs => new(lockedNums, beatDuration - currentBeatTimer, currentBeat);

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
        completionActions.Invoke();
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 8; j++)
            {
                finalPin[i, j] = Random.Range(minInclusive: 1, maxExclusive: 5);
            }
    }

    void Update()
    {
        currentBeatTimer += Time.deltaTime;
        if (!eventSent && currentBeatTimer >= stillDuration)
        {
            eventSent = true;
            OnRotate?.Invoke(this, CurrentLockEventArgs);
            return;
        }
        else if (currentBeatTimer < tfsBeatDuration) 
            return;
        
        hasLocked = false;
        eventSent = false;
        currentBeat += currentBeat is not 4 ? 1 : -3;
        for (int i = 0; i < 8; i++)
            currentPin[i] = currentBeat;

        currentBeatTimer -= beatDuration;
    }

    void Lock(InputAction.CallbackContext _)
    {
        if (hasLocked || !enabled)
            return;

        if (currentPin[lockedNums] == finalPin[lockPhase, lockedNums])
        {
            hasLocked = !continuousLockingFeature;
            if (++lockedNums is 8)
            {
                eventSent = true;
                bpm += bpmIncrease;
                RecalculateHalfBeatDuration();
                audioSource.clip = audioClips[++lockPhase];
                audioSource.Play();
                OnPhase?.Invoke(this, CurrentLockEventArgs);
            } else
                OnLock?.Invoke(this, CurrentLockEventArgs);
        }
        else
        {
            eventSent = true;
            OnUnlock?.Invoke(this, CurrentLockEventArgs);
            lockedNums = 0;
        }

        if (lockPhase is not 3 || lockedNums is not 0)
            return;

        print("Unlocked");
        enabled = false;
    }

    void RecalculateHalfBeatDuration()
    {
        lockedNums = 0;
        currentBeat = 1;
        currentBeatTimer = 0f;
        beatDuration = 60 / bpm;
        stillDuration = beatDuration * 0.5f;
        tfsBeatDuration = beatDuration * 0.75f;
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
