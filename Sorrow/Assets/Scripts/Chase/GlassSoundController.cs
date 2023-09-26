using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSoundController : MonoBehaviour
{
    void Awake()
    {
        var source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f, 1.2f);
        source.Play();
        Invoke(nameof(Destroy), source.clip.length);
    }
}
