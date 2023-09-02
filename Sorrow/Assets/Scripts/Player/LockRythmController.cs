using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockRythmController : MonoBehaviour
{
    const int totalNums = 32;
    [SerializeField] float bpm;
    readonly int[] finalPin = new int[totalNums];
    readonly int[] currentPin = new int[totalNums];
    int lockedNums = 0;
    float BeatDuration => 60 / bpm;
    int currentBeat = 1;

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
    }

    void Awake()
    {
        for (int i = 0; i < totalNums; i++)
        {
            finalPin[i] = Random.Range(minInclusive: 1, maxExclusive: 9);
            currentPin[i] = 1;
        }
    }

    IEnumerator MetronomeCoroutine()
    {
        while (true)
        {
            for (int i = lockedNums; i < totalNums; i++)
                currentPin[i] = currentBeat;
            print(string.Join("", currentPin) + "\n" + string.Join("", finalPin));
            yield return new WaitForSeconds(BeatDuration);
            currentBeat += currentBeat != 8 ? 1 : -7;
        }
    }

    void Lock(InputAction.CallbackContext _)
    {
        if (!enabled)
            return;

        if (currentPin[lockedNums] == finalPin[lockedNums])
            lockedNums++;
        else
            lockedNums -= lockedNums % 8;

        if (lockedNums is not totalNums)
            return;

        StopCoroutine(MetronomeCoroutine());
        print("Unlocked");
    }
}
