using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorClose : MonoBehaviour
{
    [SerializeField] Animator doorAnimator;

    [SerializeField] string sceneName;
    [SerializeField] int sceneLoadDelay;

    [SerializeField] bool unloadScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            doorAnimator.Play("DoorClose");
    }
    
    public IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(sceneLoadDelay);

        switch(unloadScene)
        {
            case true:
                SceneManager.UnloadSceneAsync(sceneName);
                break;
            case false:
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                break;
        }
    }
}
