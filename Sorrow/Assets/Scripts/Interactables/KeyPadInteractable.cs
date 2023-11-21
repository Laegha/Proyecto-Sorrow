using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class KeyPadInteractable : RithmInteractable
{

    [HideInInspector] public List<KeypadButton> buttons = new List<KeypadButton>();

    [SerializeField] Beat[] beats;

    int interactedTimes = 0;

    [SerializeField] GameObject[] depressionPositions;
    [SerializeField] CinemachineVirtualCamera depressionCamera;
    
    protected override void Awake()
    {
        base.Awake();
        enabled = true;
        buttons = GetComponentsInChildren<KeypadButton>().ToList();
        
        foreach (KeypadButton button in buttons)
        {
            button.keyPadInteractable = this;
            button.gameObject.SetActive(false);
        }
    }

    public override void Interaction()
    {
        if (interactedTimes > depressionPositions.Length)
            return;

        if (interactedTimes == depressionPositions.Length - 1)
        {
            base.Interaction();
            interactedTimes++;
            return;

        }

        depressionPositions[interactedTimes].SetActive(false);
        interactedTimes++;
        depressionPositions[interactedTimes].SetActive(true);
        if(interactedTimes < headphonesOffTextures.Length)
            headphonesOnMaterial.SetTexture("_MainTex", headphonesOffTextures[interactedTimes]);

        StartCoroutine(LookAtDepression());

        if (interactedTimes == depressionPositions.Length - 1)
        {
            enabled = false;
            useHeadphones = true;
        }

    }
    IEnumerator LookAtDepression()
    {
        Transform player = CinematicManager.instance.player.transform;
        var delta = new Vector2(depressionPositions[interactedTimes].transform.position.x, depressionPositions[interactedTimes].transform.position.z) - new Vector2(player.position.x, player.position.z);
        float playerRotation = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;

        Transform camera = CinematicManager.instance.playerCamera.transform;
        var camDelta = new Vector2(depressionPositions[interactedTimes].transform.position.x, depressionPositions[interactedTimes].transform.position.y) - new Vector2(camera.position.x, camera.position.y);
        float camRotation = Mathf.Atan2(camDelta.y, camDelta.x);

        depressionCamera.transform.position = camera.position;
        depressionCamera.transform.rotation = Quaternion.Euler(new Vector3(camRotation, playerRotation, 0f));
        InputManager.instance.RemRegControl(false);
        CinematicManager.instance.CameraChange(depressionCamera);

        yield return new WaitForEndOfFrame();

        while (CinematicManager.instance.cinemachineBrain.IsBlending)
            yield return new WaitForEndOfFrame();


        Rigidbody rb = player.GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.None;
        player.rotation = Quaternion.Euler(new Vector3(0f, playerRotation, 0f));

        camera.localRotation = Quaternion.Euler(new Vector3(camRotation, 0f, 0f));
        camera.GetComponent<CameraLook>().ChangeRotation(camRotation);
        CinematicManager.instance.ReturnPlayerCamera();
        InputManager.instance.RemRegControl(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        rb.interpolation = RigidbodyInterpolation.Interpolate;

    }
    public override void StartMinigame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //arrancar el tema
        foreach (KeypadButton keypadButton in buttons)
            keypadButton.gameObject.SetActive(true);
        StartCoroutine("BeatTimer");
    }

    public void RestartMinigame()
    {
        StopCoroutine("BeatTimer");
        //fade out de la música

        //devolver la camara al jugador
        CinematicManager.instance.ReturnPlayerCamera();
        InputManager.instance.RemRegControl(true);
        Cursor.lockState = CursorLockMode.Locked;

        //los botones vuelven a ponerse en verde
        buttons = GetComponentsInChildren<KeypadButton>().ToList();
        foreach (Material material in GetComponent<Renderer>().materials)
            if (material.name == "Button (Instance)")
                material.SetColor("_Color", Color.green);
    }
    IEnumerator BeatTimer()
    {
        int currBeat = 0;
        while(currBeat < beats.Length)
        {
            float deltaBeatTime = currBeat > 0 ? beats[currBeat].beatTime - beats[currBeat - 1].beatTime : beats[currBeat].beatTime;
            yield return new WaitForSeconds(deltaBeatTime);

            List<KeypadButton> availableButtons = buttons.Where(x => !x.waitingForBeat).ToList();
            if(availableButtons.Count > 0)
                StartCoroutine(availableButtons[Random.Range(0, availableButtons.Count)].WaitForBeat(beats[currBeat].beatDuration));
            currBeat++;
        }
        bool waitingForEnd = true;
        while(waitingForEnd)
        {
            yield return new WaitForEndOfFrame();

            if(!Camera.main.GetComponent<AudioSource>().isPlaying)
                waitingForEnd = false;
        }

        CinematicManager.instance.ReturnPlayerCamera();
        InputManager.instance.RemRegControl(true);
        Cursor.lockState = CursorLockMode.Locked;
        GetComponent<ElevatorArrival>().StartArrival();
        Destroy(depressionPositions[depressionPositions.Length - 1]);
    }
}

[System.Serializable]
class Beat
{
    public float beatTime;
    public float beatDuration;
}