using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorClose : MonoBehaviour
{
    [SerializeField] Animator doorAnimator;

    [SerializeField] string nextSceneName;
    [SerializeField] int sceneLoadDelay;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            doorAnimator.Play("CloseDoor");
    }
    
    public IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(sceneLoadDelay);

        SceneManager.LoadScene(nextSceneName);
    }
}
