using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBlock : HitableTarget
{
    [SerializeField] Transform particleSystemHolder;
    public override void Activate()
    {
        particleSystemHolder.parent = null;

        particleSystemHolder.GetComponent<ParticleSystem>().Play();

        Destroy(particleSystemHolder.gameObject, 5f);

        Destroy(gameObject);
    }
}
