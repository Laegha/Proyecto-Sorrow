using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] bool loadOnStart;
    [SerializeField] bool isAdditive;
    [SerializeField] string sceneToLoad;
    [SerializeField] UnityEvent onSceneLoaded;

    private void Start()
    {
        if (loadOnStart)
            LoadScene();
    }

    public void UnloadScene(string sceneName) => SceneManager.UnloadSceneAsync(sceneName);


    public void LoadScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneToLoad, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != sceneToLoad) 
            return;
            
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.SetActiveScene(scene);
        onSceneLoaded.Invoke();
    }
}
