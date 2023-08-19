using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingBox : HeldObjectNeedInteractable
{
    [SerializeField]Material boxMaterial;
    public override void Interaction()
    {
        base.Interaction();

        //hacer aparecer la caja
    }
}
