using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingNPCs : MonoBehaviour
{
    [SerializeField] Renderer[] npcs;
    Material[] materials;

    private void Awake()
    {
        materials = new Material[npcs.Length];
        for (int i = 0; i < npcs.Length; i++)
            materials[i] = npcs[i].material;
    }

    public void AppearNPCs(float appearTime) => StartCoroutine(FillAlpha(appearTime));

    IEnumerator FillAlpha(float appearTime)
    {
        float alphaClip = 1;

        while(alphaClip > 0)
        {
            alphaClip -= Time.deltaTime / appearTime;
            foreach (Material material in materials)
                material.SetFloat("_AlphaClip", alphaClip);

            yield return new WaitForEndOfFrame();
        }
    }
}
