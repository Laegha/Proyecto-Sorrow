using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadInteractable : RithmInteractable
{
    public List<KeypadButton> buttons;

    [SerializeField] Beat[] beats;

    int interactedTimes = 0;
    int InteractedTimes
    { 
        get { return interactedTimes; }
        set 
        { 
            depressionPositions[interactedTimes].SetActive(false);
            interactedTimes = value; 
            depressionPositions[interactedTimes].SetActive(true);
            CinematicManager.instance.playerCamera.LookAt = depressionPositions[interactedTimes].transform;
            CinematicManager.instance.playerCamera.LookAt = null;

        }
    }
    [SerializeField] GameObject[] depressionPositions;

    protected override void Awake()
    {
        base.Awake();
        enabled = true;
        foreach(KeypadButton button in buttons) 
            button.keyPadInteractable = this;
    }

    public override void Interaction()
    {
        if (interactedTimes > depressionPositions.Length)
            return;
        
        if(interactedTimes == depressionPositions.Length -1)
        {
            base.Interaction();
            interactedTimes++;
            return;
        
        }
        InteractedTimes++;
        
        if(interactedTimes == depressionPositions.Length -1)
        {
            enabled = false;
            useHeadphones = true;
        }

    }

    public override void StartMinigame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //arrancar el tema
        StartCoroutine(BeatTimer());
    }

    IEnumerator BeatTimer()
    {
        int currBeat = 0;
        while(currBeat < beats.Length)
        {
            float deltaBeatTime = currBeat > 0 ? beats[currBeat].beatTime - beats[currBeat - 1].beatTime : beats[currBeat].beatTime;
            yield return new WaitForSeconds(deltaBeatTime);

            StartCoroutine(buttons[Random.Range(0, buttons.Count)].WaitForBeat(beats[currBeat].beatDuration));
            currBeat++;
        }
        bool waitingForEnd = true;
        while(waitingForEnd)
        {
            yield return new WaitForEndOfFrame();

            if(!Camera.main.GetComponent<AudioSource>().isPlaying)
            {
                waitingForEnd = false;
                CinematicManager.instance.ReturnPlayerCamera();
                CinematicManager.instance.PlayerFreeze(false);
            }
        }
    }
}

[System.Serializable]
class Beat
{
    public float beatTime;
    public float beatDuration;
}