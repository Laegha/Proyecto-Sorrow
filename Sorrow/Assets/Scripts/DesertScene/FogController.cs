using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] float fadeTime;

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

    public void FogTurner(bool isEnabled) => RenderSettings.fog = isEnabled;
}
