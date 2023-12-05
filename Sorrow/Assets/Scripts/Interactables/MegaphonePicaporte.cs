using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaphonePicaporte : HeldObjectNeedInteractable
{
    [SerializeField] GameObject megaphonePicaporte;
    [SerializeField] Animator doorAnim;
    [SerializeField] AudioClip clickSound;
    [SerializeField] float delay;
    [SerializeField] AudioClip doorHandleSound;
    AudioSource[] audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponents<AudioSource>();
    }

    public override void Interaction()
    {
        base.Interaction();
        if (!neededObjectHeld)
            return;
        heldObjectManager.heldObject.gameObject.SetActive(false);
        megaphonePicaporte.SetActive(true);
        GetComponent<Animator>().Play("MegaphoneInteracted");
        foreach(SoundBullet bullet in FindObjectsOfType<SoundBullet>())
            Destroy(bullet.gameObject);
        enabled = false;
        audioSource[0].PlayOneShot(clickSound);
        Invoke(nameof(PlayDoorHandleSound), delay);
    }
    
    public void PlayDoorHandleSound() => audioSource[1].PlayOneShot(doorHandleSound);

    public void OpenDoor() => doorAnim.Play("DoorOpen");
}
