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

    void Start() => boxMaterial = GetComponent<Renderer>().material;
    
    public override void Activate()
    {
        isFilling = true;
        gameObject.layer = 0;
        gameObject.tag = "Untagged";
        Destroy(this);
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
        boxMaterial.SetFloat("_FillAmount", fillAmount);
    }
}