using System.Collections;
using System.Linq;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public Material interactionMaterial;
    HeldObjectManager playerInteraction;

    protected virtual void Awake()
    {
        playerInteraction = GameObject.FindWithTag("Player").GetComponent<HeldObjectManager>();
        interactionMaterial = GetComponent<MeshRenderer>().materials.First(m => m.name is "Outline (Instance)");
        if (!enabled) interactionMaterial.SetFloat("_CanBeInteracted", 0);
    }

    public virtual void Interaction() { }

    void OnEnable() => interactionMaterial.SetFloat("_CanBeInteracted", 1);

    void OnDisable() => interactionMaterial.SetFloat("_CanBeInteracted", 0);

    void OnMouseEnter()
    {
        if (!enabled)
            return;

        float distance = (playerInteraction.transform.position - transform.position).magnitude;
        if(distance < playerInteraction.interactionDistance)
            interactionMaterial.SetColor("_Color", Color.white);
    }

    void OnMouseExit()
    {
        if (!enabled)
            return;

        interactionMaterial.SetColor("_Color", Color.black);
    }

    private void OnMouseOver()
    {
        float distance = (playerInteraction.transform.position - transform.position).magnitude;

        if (distance > playerInteraction.interactionDistance)
            interactionMaterial.SetColor("_Color", Color.black);
        else
            interactionMaterial.SetColor("_Color", Color.white);

    }
}
