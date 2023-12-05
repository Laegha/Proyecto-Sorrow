using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleSound : RandomSoundPlayer
{
    void OnCollisionEnter(Collision _) => PlayRandomSound();
}
