using System.Collections;
using System.Linq;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public Material interactionMaterial;
    HeldObjectManager playerInteraction;

    bool canBeInteracted = true;
    public bool CanBeInteracted
    {
        get { return canBeInteracted; }
        set 
        {
            canBeInteracted = value;
            interactionMaterial.SetFloat("_CanBeInteracted", value ? 1 : 0);
        }
    }

    protected virtual void Start()
    {
        playerInteraction = GameObject.FindWithTag("Player").GetComponent<HeldObjectManager>();
        interactionMaterial = GetComponent<MeshRenderer>().materials.First(m => m.name is "InteractableMaterial (Instance)");
    }

    public virtual void Interaction() { }

    void OnMouseEnter()
    {
        if (!CanBeInteracted)
            return;

        float distance = (playerInteraction.transform.position - transform.position).magnitude;
        if(distance < playerInteraction.interactionDistance)
            interactionMaterial.SetColor("currentColor", Color.white);
    }

    void OnMouseExit()
    {
        if (!CanBeInteracted)
            return;

        float distance = (playerInteraction.transform.position - transform.position).magnitude;
        if (distance < playerInteraction.interactionDistance)
            interactionMaterial.SetColor("currentColor", Color.black);
    }
}
