using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Material interactionMaterial;
    private void Start()
    {
        interactionMaterial = GetComponent<Renderer>().material;
    }
    public virtual void Interaction()
    {

    }
    private void OnMouseEnter()
    {
        interactionMaterial.SetColor("currentColor", Color.white);
    }
    private void OnMouseExit()
    {
        interactionMaterial.SetColor("currentColor", Color.black);
        
    }
}
