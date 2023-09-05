using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    Transform[] nums;

    void Awake() => nums = GetComponentsInChildren<Transform>();

    void OnEnable()
    {
        LockRythmController.OnRotate += Rotate;
        LockRythmController.OnUnlock += UnlockNums;
    }

    void OnDisable()
    {
        LockRythmController.OnRotate -= Rotate;
        LockRythmController.OnUnlock -= UnlockNums;
    }

    void Rotate(object _, LockRythmController.LockEventArgs e)
    {
        for (int i = e.LockedNums; i < 32; i++)
            nums[i].Rotate(0f, 0f, 90f);
        // Activate the animation
    }

    void UnlockNums(object _, LockRythmController.LockEventArgs e)
    {
        float rotation = e.CurrentBeat switch
        {
            1 => 0f,
            2 => 90f,
            3 => 180f,
            4 => 270f,
            _ => 0f
        };

        for (int i = e.LockedNums; i < 32; i++)
            nums[i].Rotate(0f, 0f, rotation);
        // Animate
    }
}
