using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CsSliderController : MonoBehaviour
{
    void Awake()
    {
        var value = PlayerPrefs.GetFloat("CS", 100);
        GetComponent<Slider>().value = value;
        if (SceneManager.GetActiveScene().name == "MainMenu")
            SetCs(value);
    }

    public void ChangeCameraSensitivy(float value)
    {
        SetCs(value);
        PlayerPrefs.SetFloat("CS", value);
        PlayerPrefs.Save();
    }

    void SetCs(float value) => InputManager.cameraSensitivity = value * .001f;
}
