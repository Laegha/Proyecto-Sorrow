using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] string parameterName;
    [SerializeField] TMP_Text text;
    static AudioMixer mixer;

    void Awake()
    {
        if (mixer == null)
            mixer = Resources.Load<AudioMixer>("Mixer");
        var value = PlayerPrefs.GetFloat(parameterName, 8f);
        GetComponent<Slider>().value = value;
        text.text = $"{value}";
        if (SceneManager.GetActiveScene().name == "MainMenu")
            SetVolume(value);
    }

    public void ChangeVolume(float value)
    {
        SetVolume(value);
        text.text = $"{value}";
        PlayerPrefs.SetFloat(parameterName, value);
        PlayerPrefs.Save();
    }

    void SetVolume(float value) => mixer.SetFloat(parameterName, Mathf.Log10(value) * 80f - 80f);
}