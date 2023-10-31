using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryObjectInteractable : Interactable
{
    [SerializeField] string loadedSceneName;
    public override void Interaction()
    {
        //Brillo blanco en la camara
        //Cargar la escena
        SceneManager.LoadScene(loadedSceneName, LoadSceneMode.Single);
    }
}
