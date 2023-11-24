using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : ActionInteractable
{
    [SerializeField] Animator doorAnim;
    [SerializeField] string loadedSceneName;
    [SerializeField] bool loadScene;

    public override void Interaction()
    {
        if(loadScene)
            SceneManager.LoadScene(loadedSceneName, LoadSceneMode.Additive);
        
        doorAnim.Play("DoorOpen");
        enabled = false;
        base.Interaction();
        Destroy(this);
    }
}
