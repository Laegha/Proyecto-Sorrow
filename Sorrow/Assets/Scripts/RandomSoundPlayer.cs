using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    AudioSource audioSource;

    void Awake() => audioSource = GetComponent<AudioSource>();

    public void PlayRandomSound() => audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
}
