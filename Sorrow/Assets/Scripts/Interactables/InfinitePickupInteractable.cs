using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePickUpInteractable : Interactable
{
    [SerializeField] GameObject pickUpPrefab;

    HeldObjectManager heldObjectManager;

    private void Start() => heldObjectManager = FindObjectOfType<HeldObjectManager>();

    public override void Interaction()
    {
        GameObject pickUp = Instantiate(pickUpPrefab, Vector3.zero, Quaternion.identity);
        heldObjectManager.HoldObject(pickUp.GetComponent<HeldObject>());
    }
}
