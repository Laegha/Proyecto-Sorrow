using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    Transform[] nums;

    void Awake() => nums = GetComponentsInChildren<Transform>().Where(t => t is not RectTransform && t != transform).ToArray();

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
        float rotation = e.CurrentBeat switch
        {
            1 => 0f,
            2 => 90f,
            3 => 180f,
            4 => 270f,
            _ => 0f
        };

        print("Start rot");

        for (int i = e.LockedNums; i < 32; i++)
            nums[i].Rotate(0f, 0f, rotation, Space.Self);
        // Activate the animation

        print("End rot");
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
            nums[i].Rotate(0f, 0f, rotation, Space.Self);
        // Animate
    }
}
