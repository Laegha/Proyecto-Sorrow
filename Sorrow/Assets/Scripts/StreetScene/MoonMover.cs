using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonMover : MonoBehaviour
{
    Vector3 diff;

    void Awake() => diff = transform.position - Camera.main.transform.position;

    void Update() => transform.position = Camera.main.transform.position + diff;
}
