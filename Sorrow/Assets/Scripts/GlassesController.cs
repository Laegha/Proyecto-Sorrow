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
    bool isConcentrating, shallDisable = false;
    float timer = 1.1f;

    ColorAdjustments colorAdjustments;
    public static event System.EventHandler<bool> OnConcentrationChange;
    
    void Update()
    {
        if (timer >= 1.1f)   
        {
            enabled = !shallDisable;
            return;
        }

        timer += Time.deltaTime * .5f;
        colorAdjustments.colorFilter.value = isConcentrating ? Color.Lerp(oldColor, onColor, timer) : Color.Lerp(onColor, oldColor, timer);
    }

    void OnEnable() => ToggleConcentration(true);

    void OnDisable() => colorAdjustments.colorFilter.value = oldColor;

    public void GradialDisable()
    {
        shallDisable = true;
        ToggleConcentration(false);
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