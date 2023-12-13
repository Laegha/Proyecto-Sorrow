using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public void MovePlayer()
    {
        Transform player = CinematicManager.instance.player.transform;
        player.position = transform.position;
        player.rotation = transform.rotation;
    }
}
