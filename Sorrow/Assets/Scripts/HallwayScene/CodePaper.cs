using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class CodePaper : MonoBehaviour
{
    [SerializeField] TextMeshPro[] codeTexts;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            codeTexts[i].text = "";
            for (int j = 0; j < 8; j++)
                codeTexts[i].text += LockRhythmController.finalPin.GetValue(i, j).ToString();
        }
    }
}
