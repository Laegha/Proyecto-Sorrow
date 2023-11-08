using System.Collections;
using System.Linq;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //[HideInInspector] public Material interactionMaterial;
    HeldObjectManager playerInteraction;
    Outline outline;
    [SerializeField] Color hoverColor = Color.white;
    [SerializeField] Color outsideColor = Color.black;


    protected virtual void Awake()
    {
        playerInteraction = GameObject.FindWithTag("Player").GetComponent<HeldObjectManager>();
        //interactionMaterial = GetComponent<MeshRenderer>().materials.First(m => m.name is "Outline (Instance)");
        //if (!enabled) interactionMaterial.SetFloat("_CanBeInteracted", 0);
        outline = GetComponent<Outline>();
        outline.OutlineColor = outsideColor;
    }

    public virtual void Interaction() { }

    void OnEnable() => outline.enabled = true;

    void OnDisable() => outline.enabled = false;

    void OnMouseEnter()
    {
        if (!enabled)
            return;

        float distance = (playerInteraction.transform.position - transform.position).magnitude;
        if (distance < playerInteraction.interactionDistance)
            outline.OutlineColor = hoverColor;
        //interactionMaterial.SetColor("_Color", Color.white);
    }

    void OnMouseExit()
    {
        if (!enabled)
            return;

        //interactionMaterial.SetColor("_Color", Color.black);
        outline.OutlineColor = outsideColor;
    }

    private void OnMouseOver()
    {
        float distance = (playerInteraction.transform.position - transform.position).magnitude;

        if (distance > playerInteraction.interactionDistance)
            outline.OutlineColor = outsideColor;
        else
            outline.OutlineColor = hoverColor;

        /*
        if (distance > playerInteraction.interactionDistance)
            interactionMaterial.SetColor("_Color", Color.black);
        else
            interactionMaterial.SetColor("_Color", Color.white);
        */
    }
}
