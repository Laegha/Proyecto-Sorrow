using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaphoneTarget : MonoBehaviour
{
    public virtual void Activate()
    {
        print(name + " activated");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("MegaphoneBullet"))
            Activate();
    }

}
