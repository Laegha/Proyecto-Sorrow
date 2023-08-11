using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string sexo;
    public virtual void ItemEffect()
    {

    }
}
public class Canica : Item
{
    [SerializeField] float fuerzaTiro;
}