using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneTestingActions : MonoBehaviour
{
    [SerializeField] UnityEvent onSceneLoaded;

#if UNITY_EDITOR
    void Start()
    {
        onSceneLoaded.Invoke();
        Destroy(this);
    }
#endif
}
