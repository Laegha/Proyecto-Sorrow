using System.Linq;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    Material interactionMaterial;

    HeldObjectManager playerInteraction;
    protected virtual void Start()
    {
        playerInteraction = GameObject.FindWithTag("Player").GetComponent<HeldObjectManager>();
        interactionMaterial = GetComponent<MeshRenderer>().material; /*interactionMaterial = GetComponent<MeshRenderer>().materials.First(m => m.name is "InteractableMaterial (Instance)");*/
    }

    public virtual void Interaction() {}

    private void OnMouseEnter()
    {
        if (!enabled)
            return;

        float distance = (playerInteraction.transform.position - transform.position).magnitude;
        if(distance < playerInteraction.interactionDistance)
            interactionMaterial.SetColor("currentColor", Color.white);
    }

    private void OnMouseExit()
    {
        if (!enabled)
            return;

        float distance = (playerInteraction.transform.position - transform.position).magnitude;
        if (distance < playerInteraction.interactionDistance)
            interactionMaterial.SetColor("currentColor", Color.black);
    }
}
