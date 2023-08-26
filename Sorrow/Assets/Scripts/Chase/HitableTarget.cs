using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableTarget : MonoBehaviour
{
    [SerializeField] string incomingProyectileTag;
    public virtual void Activate()
    {
        print(name + " activated");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(incomingProyectileTag))
            Activate();
    }

}
