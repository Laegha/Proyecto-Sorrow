using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoSomethingAndDestroy : MonoBehaviour
{
    [SerializeField] UnityEvent thingsToDo;

    void Start()
    {
        thingsToDo.Invoke();
        Destroy(this);
    }
}
