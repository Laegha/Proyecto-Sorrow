using System.Linq;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    Material interactionMaterial;

    private void Start() => interactionMaterial = GetComponent<MeshRenderer>().materials.First(m => m.name is "InteractableMaterial (Instance)");

    public virtual void Interaction() {}

    private void OnMouseEnter()
    {
        if (enabled)
            interactionMaterial.SetColor("currentColor", Color.white);
    }

    private void OnMouseExit()
    {
        if (enabled)
            interactionMaterial.SetColor("currentColor", Color.black);
    }
}
