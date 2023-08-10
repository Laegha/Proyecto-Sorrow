using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] string interactableName;

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
