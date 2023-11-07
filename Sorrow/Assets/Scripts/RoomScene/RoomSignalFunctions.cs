using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSignalFunctions : MonoBehaviour
{
    [SerializeField] Vector3 position;

    public void MovePlayer() => transform.position += position;
}
