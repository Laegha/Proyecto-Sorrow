using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingBox : HitableTarget
{
    Material boxMaterial;

    float fillAmount;

    bool isFilling;
    [SerializeField] float fillMultiplier;

    private void Start() => boxMaterial= GetComponent<Renderer>().material;
    

    public override void Activate()
    {
        base.Activate();

        isFilling = true;
        gameObject.layer = 0;
        gameObject.tag = "Untagged";
    }

    void Update()
    {
        if (!isFilling || fillAmount > 1)
            return;

        fillAmount += Time.deltaTime * fillMultiplier;
        boxMaterial.SetFloat("_FillAmount", fillAmount);


    }
}
