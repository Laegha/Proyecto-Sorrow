using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanLapDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FanDetector"))
        {
            transform.parent.GetComponent<FanController>().lapDone = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
