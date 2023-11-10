using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSoundController : MonoBehaviour
{
    void Start()
    {
        var source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f, 1.2f);
        source.Play();
        Destroy(gameObject, source.clip.length);
    }
}
