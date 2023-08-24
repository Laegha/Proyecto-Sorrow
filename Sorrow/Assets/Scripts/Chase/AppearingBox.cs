using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingBox : MegaphoneTarget
{
    Material boxMaterial;

    float fillAmount;

    bool isFilling;
    [SerializeField] float fillMultiplier;

    private void Start() => boxMaterial= GetComponent<Renderer>().material;
    

    public override void Activate()
    {
        base.Activate();

        isFilling= true;
        gameObject.layer = 0;
    }

    void Update()
    {
        if (!isFilling)
            return;

        fillAmount += Time.deltaTime * fillMultiplier;
        boxMaterial.SetFloat("_FillAmount", fillAmount);
        if (fillAmount > 1)
            isFilling = false;


    }
}
