using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HallwayTimelineResources : MonoBehaviour
{
    [SerializeField] GameObject lights;
    [SerializeField] MeshRenderer[] lightObjects;
    [SerializeField] MeshRenderer[] textObjects;
    [SerializeField] Volume volume;
    [SerializeField] float offPostExposure;
    [SerializeField] PlayableDirector onOffDirector;
    [SerializeField] float minRandomSpeed, maxRandomSpeed, offTextMultiplier;
    float onPostExposure, onTextMultiplier;
    ColorAdjustments colorAdjustments;
    Material[] lightsMaterials;
    Material[] textMaterials;
    
    Color lightsOnColor;
    [SerializeField] Color lightsOffColor;

    private void Awake()
    {
        onTextMultiplier = 1f / offTextMultiplier;

        lightsMaterials = new Material[lightObjects.Length];
        for (int i = 0; i < lightObjects.Length; i++)
            lightsMaterials[i] = lightObjects[i].materials.First(m => m.name is "Lights (Instance)");
        lightsOnColor = lightsMaterials[0].GetColor("_Emission");

        foreach (MeshRenderer textObject in textObjects)
            textMaterials.Append(textObject.material);

        _ = volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        onPostExposure = colorAdjustments.postExposure.value;
    }

    public void SwitchLights(bool on)
    {
        colorAdjustments.postExposure.value = on ? onPostExposure : offPostExposure;
        lights.SetActive(on);
        foreach (Material light in lightsMaterials)
            light.SetColor("_Emission", on ? lightsOnColor : lightsOffColor);
        
        foreach (Material text in textMaterials)
            text.SetColor("_FaceColor", text.GetColor("_FaceColor") * (on ? offTextMultiplier : onTextMultiplier));
        
        var playable = onOffDirector.playableGraph.GetRootPlayable(0);
        PlayableExtensions.SetSpeed(playable, Random.Range(minRandomSpeed, maxRandomSpeed));
    }

    public void CloseElevator() => GameObject.Find("ElevatorDoor").GetComponent<Animator>().Play("DoorClose");
}
