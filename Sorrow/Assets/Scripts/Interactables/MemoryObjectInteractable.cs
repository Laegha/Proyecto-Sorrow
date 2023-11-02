using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryObjectInteractable : Interactable
{
    [SerializeField] string loadedSceneName;
    [SerializeField] Animator brightAnimator;
    public override void Interaction()
    {
        //Brillo blanco en la camara
        brightAnimator.Play("BrightAppear");
    }
}
