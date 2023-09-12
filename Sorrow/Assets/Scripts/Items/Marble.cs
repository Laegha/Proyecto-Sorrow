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
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.None;

        Quaternion rotation = Quaternion.identity;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
            rotation = Quaternion.LookRotation((hit.point - transform.position).normalized);

        transform.rotation = rotation;
        rigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }
}
