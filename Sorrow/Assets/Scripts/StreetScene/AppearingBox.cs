using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingBox : HitableTarget
{
    Material boxMaterial;
    float fillAmount;
    bool isFilling;
    [SerializeField] float fillMultiplier;
    [SerializeField] GameObject wireframe;
    [SerializeField] GameObject bullseye;
    AudioSource audioSource;

    void Start()
    {
        boxMaterial = GetComponent<Renderer>().material;
        audioSource = GetComponent<AudioSource>();
    }

    public override void Activate()
    {
        isFilling = true;
        gameObject.layer = 0;
        gameObject.tag = "Untagged";
        Destroy(bullseye);
        audioSource.Play();
    }

    void Update()
    {
        if (!isFilling )
            return;
        if(fillAmount > 1)
        {
            isFilling = false;
            Destroy(wireframe);
            return;
        }

        fillAmount += Time.deltaTime * fillMultiplier;
        Mathf.Clamp01(fillAmount);
        boxMaterial.SetFloat("_FillAmount", fillAmount);
    }
}