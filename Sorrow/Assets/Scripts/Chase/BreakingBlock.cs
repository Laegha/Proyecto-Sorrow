using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBlock : HitableTarget
{
    [SerializeField] Transform particleSystemHolder;
    static PlayerChaseMovement playerChaseMovement;

    void Awake()
    {
        if (playerChaseMovement == null)
            playerChaseMovement = FindObjectOfType<PlayerChaseMovement>();
    }

    public static void RefreshPlayer() => playerChaseMovement = null;

    public override void Activate()
    {
        particleSystemHolder.parent = null;

        particleSystemHolder.GetComponent<ParticleSystem>().Play();

        Destroy(particleSystemHolder.gameObject, 5f);

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
            playerChaseMovement.OnCollisionExit(default);
    }
}
