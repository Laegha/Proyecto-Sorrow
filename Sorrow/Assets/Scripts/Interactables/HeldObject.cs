using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObject : MonoBehaviour
{
    public string objectName;

    [HideInInspector] public Rigidbody _rigidbody;

    [HideInInspector] public MeshRenderer meshRenderer;

    [HideInInspector] public Item thisItem;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        thisItem = GetComponent<Item>();
    }
}
