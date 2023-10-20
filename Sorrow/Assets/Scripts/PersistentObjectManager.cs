using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObjectManager : MonoBehaviour
{
    public static PersistentObjectManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    [HideInInspector] public List<PersistentObject> persistentObjects = new List<PersistentObject>();

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (PersistentObject persistentObject in persistentObjects)
            if (scene.name == persistentObject.destroySceneName)
                Destroy(persistentObject.gameObject);
    }
}
