using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlassesController : MonoBehaviour
{
    [SerializeField] Color onColor;
    Color oldColor;
    bool isConcentrating = false;
    float timer = 1.1f;

    ColorAdjustments colorAdjustments;
    public static event System.EventHandler<bool> OnConcentrationChange;
    
    void Update()
    {
        if (timer >= 1.1f)
            return;

        timer += Time.deltaTime * .5f;
        colorAdjustments.colorFilter.value = isConcentrating ? Color.Lerp(oldColor, onColor, timer) : Color.Lerp(onColor, oldColor, timer);
    }

    void OnEnable() => ToggleConcentration(true);

    void OnDisable() => ToggleConcentration(false);

    public void FakeDisable()
    {
        OnDisable();
        StartCoroutine(GreedyUpdate()); // Not Proud of this
    }

    IEnumerator GreedyUpdate()
    {
        while (timer < 1.1f)
        {
            Update();
            yield return new WaitForEndOfFrame();
        }
        enabled = false;
        StopCoroutine(GreedyUpdate());
    }

    void ToggleConcentration(bool on)
    {
        isConcentrating ^= true;
        timer = 0f;
        OnConcentrationChange?.Invoke(this, isConcentrating);

        if (!isConcentrating)
            return;

        var volume = GameObject.Find("PostProcessingURP").GetComponent<Volume>();
        if (!volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            colorAdjustments = volume.profile.Add<ColorAdjustments>(true);
        
        oldColor = colorAdjustments.colorFilter.value;
    }
}