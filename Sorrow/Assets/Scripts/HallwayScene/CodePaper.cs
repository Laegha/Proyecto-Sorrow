using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class CodePaper : MonoBehaviour
{
    [SerializeField] TextMeshPro[] codeTexts;
    float timer, timeMultiplier, initRotation, finalRotation = 0f;
    HeldObjectManager heldObjectManager;

    void Start()
    {
        heldObjectManager = FindObjectOfType<HeldObjectManager>();
        LockRhythmController.OnPhase += SetupRotation;
        for (int i = 0; i < 4; i++)
        {
            codeTexts[i].text = "";
            for (int j = 0; j < 8; j++)
                codeTexts[i].text += LockRhythmController.finalPin.GetValue(i, j).ToString();
        }
    }

    void OnDestroy() 
        => LockRhythmController.OnPhase -= SetupRotation;

    void Update()
    {
        if (timer >= 1.1f)
            return;

        timer += Time.deltaTime * timeMultiplier;
        var rotation = transform.eulerAngles;
        rotation.z = EaseOutExpo(initRotation, finalRotation, timer);
        transform.eulerAngles = rotation;
    }

    public void SetTransform(Transform t)
    {
        heldObjectManager.ReleaseObject();
        transform.SetPositionAndRotation(t.position, t.rotation);
        transform.localScale = t.localScale;
    }

    void SetupRotation(object _, float inTime)
    {
        timer = 0f;
        timeMultiplier = 1f / inTime;
        initRotation = transform.eulerAngles.z;
        finalRotation = initRotation - 90f;
    }

    float EaseOutExpo(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);

        end -= start;
        return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
    }
}
