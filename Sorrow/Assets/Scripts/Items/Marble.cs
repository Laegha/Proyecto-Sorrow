using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : Item
{
    [SerializeField] int throwForce;
    public override void ItemEffect()
    {
        transform.SetParent(null);
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }
}
