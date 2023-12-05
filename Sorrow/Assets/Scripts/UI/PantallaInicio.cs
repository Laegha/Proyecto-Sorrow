using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PantallaInicio : MonoBehaviour
{
    //GameObject opciones;
    [SerializeField] GameObject pantallaOpciones;
    [SerializeField] GameObject blackPanel;
    [SerializeField] float fadeSpeed = 1f;
    Image blackScreen;
    bool starting = false;
    float timer = 0f;

    void Awake() => blackScreen = blackPanel.GetComponent<Image>();

    //void Awake()
    //{
    //    opciones = GameObject.Fin("PantallaOpciones"); (no funca, sad)
    //}
    public void StartGame()
    {
        print("Load StreetScene");
        blackPanel.SetActive(true);
        starting = true;
    }

    void LoadStreetScene() => SceneManager.LoadScene("StreetScene");

    void Update()
    {
        if (!starting)
            return;

        blackScreen.color = new(0f, 0f, 0f, Mathf.Clamp01(timer));
        timer += Time.deltaTime * fadeSpeed;

        if (timer <= 1.1f)
            return;
        
        starting = false;
        LoadStreetScene();
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
