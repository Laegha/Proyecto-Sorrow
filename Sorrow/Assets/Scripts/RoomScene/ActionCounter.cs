using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionCounter : MonoBehaviour
{
    int doneActions = 0;
    [HideInInspector] public int neededActions;
    [SerializeField] UnityEvent completedActions;

    public void ActionDone()
    {
        doneActions++;
        if (doneActions != neededActions)
            return;
            
        completedActions.Invoke();
        Destroy(gameObject);
    }
}
