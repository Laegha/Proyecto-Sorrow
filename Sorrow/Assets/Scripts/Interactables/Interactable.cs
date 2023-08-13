using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interaction()
    {

    }
    private void OnMouseEnter()
    {
        print("Can interact with object " + transform.name);
    }
    private void OnMouseExit()
    {
        print("Can not interact with object " + transform.name);
        
    }
}
