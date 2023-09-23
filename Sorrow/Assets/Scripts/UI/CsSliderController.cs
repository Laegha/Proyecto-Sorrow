using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CsSliderController : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    void Awake()
    {
        var value = PlayerPrefs.GetFloat("CS", 100f);
        GetComponent<Slider>().value = value;
        text.text = $"{value * .01f}";
        if (SceneManager.GetActiveScene().name == "MainMenu")
            SetCs(value);
    }

    public void ChangeCameraSensitivy(float value)
    {
        SetCs(value);
        text.text = $"{value * .01f}";
        PlayerPrefs.SetFloat("CS", value);
        PlayerPrefs.Save();
    }

    void SetCs(float value) => InputManager.cameraSensitivity = value * .001f;
}
