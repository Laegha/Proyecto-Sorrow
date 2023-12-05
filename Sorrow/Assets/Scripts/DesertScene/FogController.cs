using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] float fadeTime;
    float fogStartDistance, fogEndDistance, fogDensity;
    Color fogColor;
    FogMode fogMode;

    /*
    private void OnTriggerEnter(Collider other) => StartCoroutine(FogFade());

    IEnumerator FogFade()
    {
        while(RenderSettings.fogStartDistance < 40)
        {
            RenderSettings.fogStartDistance += Time.deltaTime * 40 / fadeTime;
            RenderSettings.fogEndDistance += Time.deltaTime * 40 / fadeTime;
            yield return new WaitForEndOfFrame();
        }
    }
    */

    void Start()
    {
        RenderSettings.fog = false;
        fogStartDistance = RenderSettings.fogStartDistance;
        fogEndDistance = RenderSettings.fogEndDistance;
        fogDensity = RenderSettings.fogDensity;
        fogColor = RenderSettings.fogColor;
        fogMode = RenderSettings.fogMode;
    }

    public void FogTurner(bool isEnabled)
    {
        RenderSettings.fog = isEnabled;
        RenderSettings.fogStartDistance = fogStartDistance;
        RenderSettings.fogEndDistance = fogEndDistance;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = fogMode;
    }
}
