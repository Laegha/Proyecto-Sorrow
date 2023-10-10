using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

            Transform player = CinematicManager.instance.player.transform;
            var delta = new Vector2(depressionPositions[interactedTimes].transform.position.x, depressionPositions[interactedTimes].transform.position.z) - new Vector2(player.position.x, player.position.z);
            player.rotation = Quaternion.Euler(new Vector3(0f, Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg, 0f));

            Transform camera = CinematicManager.instance.playerCamera.transform;
            var camDelta = new Vector2(depressionPositions[interactedTimes].transform.position.x, depressionPositions[interactedTimes].transform.position.y) - new Vector2(camera.position.x, camera.position.y);
            float camRotation = Mathf.Atan2(camDelta.y, camDelta.x);
            camera.localRotation = Quaternion.Euler(new Vector3(camRotation, 0f, 0f));
            camera.GetComponent<CameraLook>().ChangeRotation(camRotation);
            print(camRotation);
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
        foreach (KeypadButton keypadButton in buttons)
            keypadButton.gameObject.SetActive(true);
        StartCoroutine(BeatTimer());
    }

    IEnumerator BeatTimer()
    {
        int currBeat = 0;
        while(currBeat < beats.Length)
        {
            float deltaBeatTime = currBeat > 0 ? beats[currBeat].beatTime - beats[currBeat - 1].beatTime : beats[currBeat].beatTime;
            yield return new WaitForSeconds(deltaBeatTime);

            List<KeypadButton> availableButtons = buttons.Where(x => !x.waitingForBeat).ToList();
            StartCoroutine(availableButtons[Random.Range(0, availableButtons.Count)].WaitForBeat(beats[currBeat].beatDuration));
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