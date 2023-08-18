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
            InputManager.controller.PlayerRun.Enable();
            InputManager.instance.GetComponent<PlayerChaseMovement>().enabled = true;
            Debug.Log("RUN");
        } else
        {
            InputManager.controller.PlayerRun.Disable();
            InputManager.instance.GetComponent<PlayerChaseMovement>().enabled = false;
            Debug.Log("WALK");
            timesInteracted = 0;
        }
    }
}
