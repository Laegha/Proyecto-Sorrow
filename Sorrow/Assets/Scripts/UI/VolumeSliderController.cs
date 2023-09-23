using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] string parameterName;
    static AudioMixer mixer;

    void Awake()
    {
        if (mixer == null)
            mixer = Resources.Load<AudioMixer>("Mixer");
        var value = PlayerPrefs.GetFloat(parameterName, 8);
        GetComponent<Slider>().value = value;
        if (SceneManager.GetActiveScene().name == "MainMenu")
            SetMixer(value);
    }

    public void ChangeVolume(float value)
    {
        SetMixer(value);
        PlayerPrefs.SetFloat(parameterName, value);
        PlayerPrefs.Save();
    }

    void SetMixer(float value) => mixer.SetFloat(parameterName, Mathf.Log10(value) * 80f - 80f);
}