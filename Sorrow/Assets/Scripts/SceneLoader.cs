using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] bool loadOnStart;
    [SerializeField] string sceneToLoad;

    private void Start()
    {
        if (loadOnStart)
            LoadScene();
    }

    void LoadScene() => SceneManager.LoadScene(sceneToLoad);
}
