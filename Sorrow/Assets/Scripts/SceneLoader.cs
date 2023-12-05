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

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        if (isAdditive)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != sceneToLoad) 
            return;
            
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.SetActiveScene(scene);
    }
}
