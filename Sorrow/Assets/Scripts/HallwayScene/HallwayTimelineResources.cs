using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HallwayTimelineResources : MonoBehaviour
{
    [SerializeField] GameObject lights;
    [SerializeField] MeshRenderer[] lightObjects;
    [SerializeField] TextMeshPro[] textObjects;
    [SerializeField] Volume volume;
    [SerializeField] float offPostExposure;
    [SerializeField] PlayableDirector onOffDirector;
    [SerializeField] float minRandomSpeed, maxRandomSpeed, offTextMultiplier;
    float onPostExposure, onTextMultiplier;
    bool textAnnoyance = true;
    ColorAdjustments colorAdjustments;
    Material[] lightsMaterials;
    
    Color lightsOnColor;
    [SerializeField] Color lightsOffColor;

    void Awake()
    {
        onTextMultiplier = 1f / offTextMultiplier;

        lightsMaterials = new Material[lightObjects.Length];
        for (int i = 0; i < lightObjects.Length; i++)
            lightsMaterials[i] = lightObjects[i].materials.First(m => m.name is "Lights (Instance)");
        lightsOnColor = lightsMaterials[0].GetColor("_Emission");

        _ = volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        onPostExposure = colorAdjustments.postExposure.value;
        GlassesController.OnConcentrationChange += PutOnGlasses;
    }

    void OnDestroy() => GlassesController.OnConcentrationChange -= PutOnGlasses;

    void PutOnGlasses(object _, bool isConcentrating)
    {
        textAnnoyance = !isConcentrating;
        colorAdjustments.postExposure.value = isConcentrating ? onPostExposure : offPostExposure;
        if (!lights.activeSelf)
            foreach (TextMeshPro text in textObjects)
                text.color *= isConcentrating ? onTextMultiplier : offTextMultiplier;
    }

    public void SwitchLights(bool on)
    {
        lights.SetActive(on);
        foreach (Material light in lightsMaterials)
            light.SetColor("_Emission", on ? lightsOnColor : lightsOffColor);
        
        if (textAnnoyance)
        {
            colorAdjustments.postExposure.value = on ? onPostExposure : offPostExposure;

            foreach (TextMeshPro text in textObjects)
                text.color *= on ? onTextMultiplier : offTextMultiplier;
        }
        
        var playable = onOffDirector.playableGraph.GetRootPlayable(0);
        PlayableExtensions.SetSpeed(playable, Random.Range(minRandomSpeed, maxRandomSpeed));
    }

    public void FinalSwitchOn()
    {
        bool wasOn = lights.activeSelf;
        lights.SetActive(true);
        foreach (Material light in lightsMaterials)
            light.SetColor("_Emission", lightsOnColor);

        colorAdjustments.postExposure.value = onPostExposure;
        if (!wasOn)
            foreach (TextMeshPro text in textObjects)
                text.color *= onTextMultiplier;
    }

    public void CloseElevator() => GameObject.Find("ElevatorDoor").GetComponent<Animator>().Play("DoorClose");
}
