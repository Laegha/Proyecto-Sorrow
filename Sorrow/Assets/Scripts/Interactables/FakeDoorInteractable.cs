using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDoorInteractable : Interactable
{
    int timesInteracted = 0;

    public override void Interaction()
    {
        if (timesInteracted < 2)
        {
            timesInteracted++;
            Debug.Log($"Interacted {timesInteracted}");
            return;
        }

        if (!InputManager.controller.PlayerRun.enabled)
        {
            InputManager.instance.GetComponent<PlayerChaseMovement>().enabled = true;
            InputManager.instance.GetComponent<PlayerMovement>().enabled = false;
            Debug.Log("RUN");
        } 
        else
        {
            InputManager.instance.GetComponent<PlayerChaseMovement>().enabled = false;
            Debug.Log("WALK");
            timesInteracted = 0;
        }
    }
}
