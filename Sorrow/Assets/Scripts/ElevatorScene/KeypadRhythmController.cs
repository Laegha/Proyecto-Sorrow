using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class KeypadRhythmController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera keypadCamera;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ElevatorArrival elevatorArrival;
    PlayableDirector playableDirector;
    KeypadButton[] buttons;
    GlassesController glassesController;
    int buttonsLeft = 9;

    void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        buttons = GetComponentsInChildren<KeypadButton>();
        elevatorArrival = GetComponent<ElevatorArrival>();
    }

    void OnEnable()
    {
        foreach (KeypadButton keypadButton in buttons)
            keypadButton.enabled = true;

        glassesController = FindObjectOfType<GlassesController>();
        glassesController.enabled = true;
        InputManager.controller.Movement.Disable();
        CinematicManager.instance.CameraChange(keypadCamera);
        KeypadButton.OnMiss += OnMiss;
        playableDirector.stopped += Win;
        playableDirector.Play();
    }

    void OnDisable()
    {
        foreach (KeypadButton keypadButton in buttons)
        {
            keypadButton.enabled = false;
            keypadButton.RestoreColor();
        }

        glassesController.enabled = false;
        InputManager.controller.Movement.Enable();
        CinematicManager.instance.ReturnPlayerCamera();
        elevatorArrival.StartArrival();
    }

    public void CreateBeat()
    {
        var array = buttons.Where(x => x.enabled && !x.waitingForBeat).ToArray();
        if (array.Length is not 0)
            array[Random.Range(0, buttons.Length)].WaitForBeat();
    }

    void OnMiss(object _, System.EventArgs __)
    {
        if (--buttonsLeft is not 0)
            return;

        playableDirector.Play();
        CinematicManager.instance.StartCameraShake(4f);
        audioSource.Play();

        Invoke(nameof(Restart), audioSource.clip.length);
    }

    void Restart()
    {
        CinematicManager.instance.StopCameraShake();
        buttonsLeft = 9;
        foreach (KeypadButton keypadButton in buttons)
            keypadButton.enabled = true;
        playableDirector.Play();
    }

    void Win(PlayableDirector _) => enabled = false;
}
