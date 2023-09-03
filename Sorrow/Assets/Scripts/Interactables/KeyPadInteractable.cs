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
        //Cursor.lockState = CursorLockMode.Confined;
        //arrancar el tema
        StartCoroutine(BeatTimer());
    }

    IEnumerator BeatTimer()
    {
        int currBeat = 0;
        while(currBeat < beats.Length)
        {
            yield return new WaitForSeconds(beats[currBeat].beatTime);

        }
    }
}

[System.Serializable]
public class Beat
{
    public float beatTime;
}