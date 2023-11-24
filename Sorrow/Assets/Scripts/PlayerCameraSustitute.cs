using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraSustitute : MonoBehaviour
{
    public void RecolocateCamera()
    { 
        transform.position = CinematicManager.instance.playerCamera.transform.position;
        transform.rotation = CinematicManager.instance.playerCamera.transform.rotation;
    }
}
