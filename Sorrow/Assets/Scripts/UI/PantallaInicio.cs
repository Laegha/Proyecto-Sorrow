using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaInicio : MonoBehaviour
{
    //GameObject opciones;
    [SerializeField] GameObject pantallaOpciones;
    //void Awake()
    //{
    //    opciones = GameObject.Fin("PantallaOpciones"); (no funca, sad)
    //}
    public void StartGame()
    {
        print("Load StreetScene");
        SceneManager.LoadScene("StreetScene");
    }

    public void OptionsButton()
    {
        print("Loaded Options menu!");
        gameObject.SetActive(false);
        pantallaOpciones.SetActive(true);

    }
    public void ExitGame()
    {
        print("Goodbye World!");
        Application.Quit();
    }
}
