using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HallwayTimelineResources : MonoBehaviour
{
    [SerializeField] GameObject lights;
    [SerializeField] MeshRenderer[] lightObjects;
    Material[] lightsMaterials;
    
    Color lightsOnColor;
    [SerializeField] Color lightsOffColor;

    private void Awake()
    {
        lightsMaterials = new Material[lightObjects.Length];
        for (int i = 0; i < lightObjects.Length; i++)
            lightsMaterials[i] = lightObjects[i].materials.First(m => m.name is "Lights (Instance)");
        lightsOnColor = lightsMaterials[0].GetColor("_Emission");
    }

    public void SwitchLights(bool on)
    {
        lights.SetActive(on);
        foreach (Material light in lightsMaterials)
            light.SetColor("_Emission", on ? lightsOnColor : lightsOffColor);
    }

    public void CloseElevator() => GameObject.Find("ElevatorDoor").GetComponent<Animator>().Play("DoorClose");
}
