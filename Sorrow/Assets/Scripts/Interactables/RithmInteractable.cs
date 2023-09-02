using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class RithmInteractable : Interactable
{
    //public int empanadas;//esto es un meme
    [SerializeField] CinemachineVirtualCamera interactableCamera;

    [SerializeField] Texture3D headphonesOnTexture;
    [SerializeField] Texture3D headphonesOffTexture;

    [SerializeField] string headphonesOnTextureName;

    Material headphonesOnMaterial;
    protected override void Start()
    {
        base.Start();

        headphonesOnMaterial = GetComponent<MeshRenderer>().materials.First(m => m.name == headphonesOnTextureName + " (Instance)");
        HeadphoneController.rithmInteractables.Add(this);
    }

    public override void Interaction()
    {
        base.Interaction();

        CinematicManager.CameraChange(interactableCamera);
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
    }

    public void SwitchCurrState(bool canBeInteracted)
    {
        enabled = canBeInteracted;
        headphonesOnMaterial.SetTexture("_MainTex", canBeInteracted ? headphonesOnTexture : headphonesOffTexture);
        interactionMaterial.SetFloat("_CanBeInteracted", canBeInteracted ? 1 : 0);
    }
}
