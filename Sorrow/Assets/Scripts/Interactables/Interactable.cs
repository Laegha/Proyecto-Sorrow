using System.Collections;
using System.Linq;
using UnityEngine;

public class Interactable : MonoBehaviour
{
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
        outline = GetComponent<Outline>();
        outline.OutlineColor = outsideColor;
        outline.enabled = enabled;
        playerStillPlacement = Camera.main.transform.position;
        thisStillPlacement = transform.position;
    }

    public virtual void Interaction() { }

    void OnEnable() => outline.enabled = true;

    void OnDisable() => outline.enabled = false;

    void OnMouseEnter()
    {
        if (!enabled)
            return;

        if (Vector3.Distance(Camera.main.transform.position, transform.position) < interactionDistance)
            outline.OutlineColor = hoverColor;
    }

    void OnMouseExit()
    {
        if (!enabled)
            return;

        if (Vector3.Distance(Camera.main.transform.position, transform.position) <= distanceOffset)
            outline.OutlineColor = outsideColor;
    }

    void OnMouseOver()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (distance > distanceOffset)
            return;

        if (distance > interactionDistance)
            outline.OutlineColor = outsideColor;
        else
            outline.OutlineColor = hoverColor;
    }

    void Update()
    {
        if (playerStillPlacement == Camera.main.transform.position && thisStillPlacement == transform.position)
            return;

        playerStillPlacement = Camera.main.transform.position;
        thisStillPlacement = transform.position;

        float distance = Vector3.Distance(Camera.main.transform.position, transform.position) - distanceOffset;
        
        // Color solution
        distance *= distanceMultiplier;
        distance = Mathf.Clamp01(distance);
        Color color = outline.OutlineColor;
        color.a = 1f - distance;
        outline.OutlineColor = color;
    }

    public void ChangeOutlineHoverColor(Color newColor) => hoverColor = newColor;
}
