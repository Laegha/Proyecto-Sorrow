using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct ActionInterrupt
{
    public int at;
    public UnityEvent actions;
}
