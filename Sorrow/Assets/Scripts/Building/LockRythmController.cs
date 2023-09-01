using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRythmController : MonoBehaviour
{
    [SerializeField] float bpm;
    readonly int[] finalPin = new int[16];
    readonly int[] currentPin = new int[16];
    int lockedNums = 0;
    float BeatDuration => 60 / bpm;
    int currentBeat = 1;

    void Awake()
    {
        for (int i = 0; i < 16; i++)
        {
            finalPin[i] = Random.Range(minInclusive: 1, maxExclusive: 9);
            currentPin[i] = 1;
        }
    }

    IEnumerator MetronomeCoroutine()
    {
        while (true)
        {
            for (int i = lockedNums; i < 16; i++)
                currentPin[i] = currentBeat;
            Debug.Log(string.Join("", currentPin) + "\n" + string.Join("", finalPin));
            yield return new WaitForSeconds(BeatDuration);
            currentBeat += currentBeat != 8 ? 1 : -7;
        }
    }

    public void StartMetronome() => StartCoroutine(MetronomeCoroutine());

    public void Lock()
    {
        if (currentPin[lockedNums] == finalPin[lockedNums])
            lockedNums++;
        else
            lockedNums -= lockedNums % 4;

        if (lockedNums is 16)
        {
            StopCoroutine(MetronomeCoroutine());
            Debug.Log("Unlocked");
        }
    }
}
