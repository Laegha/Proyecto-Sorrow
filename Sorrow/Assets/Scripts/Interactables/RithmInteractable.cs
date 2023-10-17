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
    [SerializeField] string headphonesOnMaterialName;

    Material headphonesOnMaterial;

    [HideInInspector] public bool useHeadphones = false;

    protected override void Awake()
    {
        base.Awake();
        
        headphonesOnMaterial = GetComponent<MeshRenderer>().materials.First(m => m.name == headphonesOnMaterialName + " (Instance)");
        
        HeadphoneController.rithmInteractables.Add(this);

        enabled = false;
    }

    public override void Interaction()
    {
        if (!enabled)
        {
            print("No se puede interactuar");
            return;
        }

        CinematicManager.instance.CameraChange(interactableCamera);
        InputManager.instance.RemRegControl(false);
        //CinematicManager.instance.PlayerFreeze(true);

        enabled = false;
        StartMinigame();
    }

    public virtual void StartMinigame() { }

    public void SwitchCurrState(bool canBeInteracted)
    {
        enabled = canBeInteracted;
        print("El objeto " + name + " está " + enabled);
        headphonesOnMaterial.SetTexture("_MainTex", canBeInteracted ? headphonesOnTexture : headphonesOffTexture);
    }
}
