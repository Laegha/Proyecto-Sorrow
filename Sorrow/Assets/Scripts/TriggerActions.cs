using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerActions : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;

    void OnTriggerEnter(Collider other)
    {
        if (enabled && other.CompareTag("Player"))
            onTriggerEnter.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (enabled && other.CompareTag("Player"))
            onTriggerExit.Invoke();
    }
}
