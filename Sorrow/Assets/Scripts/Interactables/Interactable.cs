using System.Collections;
using System.Linq;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //[HideInInspector] public Material interactionMaterial;
    HeldObjectManager playerInteraction;
    public float interactionDistance = 1.0f;
    Outline outline;
    [SerializeField] Color hoverColor = Color.white;
    [SerializeField] Color outsideColor = Color.black;
    Vector3 playerStillPlacement;
    Vector3 thisStillPlacement;
    [SerializeField] float distanceMultiplier = 0.1f;
    [SerializeField] float distanceOffset = 10f;

    protected virtual void Awake()
    {
        playerInteraction = GameObject.FindWithTag("Player").GetComponent<HeldObjectManager>();
        outline = GetComponent<Outline>();
        outline.OutlineColor = outsideColor;
        outline.enabled = enabled;
        playerStillPlacement = playerInteraction.transform.position;
        thisStillPlacement = transform.position;
    }

    public virtual void Interaction() { }

    void OnEnable() => outline.enabled = true;

    void OnDisable() => outline.enabled = false;

    void OnMouseEnter()
    {
        if (!enabled)
            return;

        float distance = (playerInteraction.transform.position - transform.position).magnitude;
        if (distance < interactionDistance)
            outline.OutlineColor = hoverColor;
    }

    void OnMouseExit()
    {
        if (!enabled)
            return;

        float distance = (playerInteraction.transform.position - transform.position).magnitude;
        if (distance <= distanceOffset)
            outline.OutlineColor = outsideColor;
    }

    void OnMouseOver()
    {
        float distance = (playerInteraction.transform.position - transform.position).magnitude;

        if (distance > distanceOffset)
            return;

        if (distance > interactionDistance)
            outline.OutlineColor = outsideColor;
        else
            outline.OutlineColor = hoverColor;
    }

    void Update()
    {
        if (playerStillPlacement == playerInteraction.transform.position && thisStillPlacement == transform.position)
            return;

        playerStillPlacement = playerInteraction.transform.position;
        thisStillPlacement = transform.position;

        float distance = (playerInteraction.transform.position - transform.position).magnitude - distanceOffset;
        
        // Color solution
        distance *= distanceMultiplier;
        distance = Mathf.Clamp(distance, 0f, 1f);
        Color color = outline.OutlineColor;
        color.a = 1f - distance;
        outline.OutlineColor = color;
    }
}
