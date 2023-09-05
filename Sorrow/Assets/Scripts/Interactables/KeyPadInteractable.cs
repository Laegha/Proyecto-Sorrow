using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadInteractable : RithmInteractable
{
    KeypadButton[] buttons => GetComponentsInChildren<KeypadButton>();

    [SerializeField] Beat[] beats;

    protected override void Start()
    {
        base.Start();

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

            StartCoroutine(buttons[Random.Range(0, buttons.Length)].WaitForBeat(beats[currBeat].beatDuration));
            currBeat++;
        }
    }
}

[System.Serializable]
class Beat
{
    public float beatTime;
    public float beatDuration;
}