using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFallDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monster")) return;

        Destroy(other.GetComponent<Rigidbody>());
        Destroy(gameObject);
    }
}
