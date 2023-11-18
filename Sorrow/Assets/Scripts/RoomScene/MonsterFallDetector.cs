using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFallDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            other.transform.rotation = Quaternion.Euler(other.transform.rotation.x, -74, 180);
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.GetComponent<Collider>().isTrigger = false;
            Destroy(gameObject);
        }
    }
}
