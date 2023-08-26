using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBlock : MegaphoneTarget
{
    [SerializeField] Transform particleSystemHolder;
    public override void Activate()
    {
        particleSystemHolder.parent = null;

        particleSystemHolder.GetComponent<ParticleSystem>().Play();

        Destroy(particleSystemHolder.gameObject, 5);

        Destroy(gameObject);
    }
}
