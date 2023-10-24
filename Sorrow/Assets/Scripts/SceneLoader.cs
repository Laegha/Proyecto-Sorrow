using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] bool loadOnStart;
    [SerializeField] bool isAdditive;

    [SerializeField] string sceneToLoad;

    private void Start()
    {
        if (loadOnStart)
            LoadScene();
    }

    public void LoadScene() => SceneManager.LoadScene(sceneToLoad, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
}
