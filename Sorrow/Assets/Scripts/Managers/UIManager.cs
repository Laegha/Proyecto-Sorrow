using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    [SerializeField] Image staminaBarImage;

    public void UpdateStaminaBar(float value)
    {
        staminaBarImage.fillAmount = value;
        print(staminaBarImage.fillAmount);
    }

    void ShowTutorialLabel()
    {

    }
}
