using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadInteractable : RithmInteractable
{
    public List<KeypadButton> buttons;

    [SerializeField] Beat[] beats;

    protected override void Awake()
    {
        base.Awake();

        foreach(KeypadButton button in buttons) 
            button.keyPadInteractable = this;
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
                CinematicManager.ReturnPlayerCamera();
                CinematicManager.PlayerFreeze(false);
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