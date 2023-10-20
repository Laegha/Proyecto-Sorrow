using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    public string destroySceneName;

    private void Start()
    {
        transform.parent = null;
        DontDestroyOnLoad(this);
        PersistentObjectManager.instance.persistentObjects.Add(this);
    }
}
