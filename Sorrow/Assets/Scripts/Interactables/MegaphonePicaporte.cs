using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaphonePicaporte : HeldObjectNeedInteractable
{
    [SerializeField] GameObject megaphonePicaporte;
    [SerializeField] Animator doorAnim;
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
    }

    public void OpenDoor() => doorAnim.Play("DoorOpen");
}
