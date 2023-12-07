using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using Cinemachine;

public class DialogDriver : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject speechPanel;
    [SerializeField] GameObject transcriptPanel;
    [SerializeField] Animator clickAnimator;
    [SerializeField] Animator autoAnimator;
    TMP_Text speech;
    ScrollRect transcript;

    [Header("Dialog")]
    [SerializeField] Dialog dialog;
    [SerializeField] Color playerColor;
    [SerializeField] CinemachineVirtualCamera defaultCamera;

    [Header("Actions")]
    [SerializeField] List<ActionInterrupt> actionInterrupts;
    [SerializeField] UnityEvent endActions;

    const float comaTime = .2f;
    const float semiColonTime = .35f;
    const float periodTime = .5f;
    const float letterTime = 0.03f;
    int currentLine = 0;
    PlayableDirector director;
    bool wishToSkip = false;
    bool isSpeaking = false;
    bool auto = false;

    GameObject player;
    PlayerMovement playerMovement;
    CameraLook cameraLook;
    CinemachineVirtualCamera oldCamera;

    void Awake()
    {
        dialog.SetLanguage();
        director = GetComponent<PlayableDirector>();
        //letterTime = LetterTimeFor(LocalizationSettings.SelectedLocale.Identifier.Code);
        speech = speechPanel.GetComponentInChildren<TMP_Text>();
        transcript = transcriptPanel.GetComponentInChildren<ScrollRect>();
        LocalizationSettings.SelectedLocaleChanged += UpdateLocaleSpeed;
    }

    void OnEnable()
    {   
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        cameraLook = player.GetComponentInChildren<CameraLook>();
        StartCoroutine(Initialize());
    }

    void OnDisable()
    {
        InputManager.controller.Dialog.Auto.performed -= SetAuto;
        InputManager.controller.Dialog.Continue.performed -= Continue;
        InputManager.controller.Dialog.ShowTranscript.performed -= OpenTranscript;
        InputManager.controller.Dialog.Disable();
        playerMovement.enabled = true;
        cameraLook.enabled = true;
        if (oldCamera)
            CinematicManager.instance.CameraChange(oldCamera);
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        playerMovement.enabled = false;
        cameraLook.enabled = false;
        if (defaultCamera)
        {
            oldCamera = CinematicManager.instance.CurrCamera;
            CinematicManager.instance.CameraChange(defaultCamera);
        }
        InputManager.controller.Dialog.Enable();
        InputManager.controller.Dialog.Auto.performed += SetAuto;
        InputManager.controller.Dialog.Continue.performed += Continue;
        InputManager.controller.Dialog.ShowTranscript.performed += OpenTranscript;
        StartCoroutine(StartingCoroutine());
    }

    void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= UpdateLocaleSpeed;

    IEnumerator StartingCoroutine()
    {
        if (dialog.preTimeline)
        {
            director.Play(dialog.preTimeline);
            yield return new WaitForSeconds((float)dialog.preTimeline.duration);
        }
        // TODO: Open UI with animation
        speechPanel.SetActive(true); // DEBUG
        yield return MainLoop();
    }

    void SetAuto(InputAction.CallbackContext _)
    {
        auto ^= true;
        autoAnimator.SetBool("isOn", auto);
        if (!isSpeaking && director.state != PlayState.Playing)
            clickAnimator.SetBool("isOn", auto);

        if (auto && !isSpeaking && director.state != PlayState.Playing)
            StartCoroutine(MainLoop());
    }

    void Continue(InputAction.CallbackContext _)
    {
        if (isSpeaking)
            wishToSkip = true;
        else if (director.state != PlayState.Playing)
            StartCoroutine(MainLoop());
    }

    void OpenTranscript(InputAction.CallbackContext _)
    {
        // TODO: Close Speech UI with animation
        speechPanel.SetActive(false); // DEBUG
        // TODO: Open Transcript UI with animation
        transcript.verticalNormalizedPosition = 1;
        transcriptPanel.SetActive(true); // DEBUG
    }

    int currInterrupt = 0;
    IEnumerator MainLoop()
    {
        if (currentLine < dialog.Count)
        {
            clickAnimator.SetBool("isOn", false);
            ActionInterrupt actions = actionInterrupts.FirstOrDefault(x => x.at == currentLine);
            if (actions.actions != default)
                actions.actions.Invoke();
            if (dialog.TryInterrupt(currentLine, out var timeline, out var dontStopText, currInterrupt))
            {
                director.Play(timeline);
                currInterrupt++;
                if (!dontStopText)
                {
                    // TODO: Close UI with animation
                    speechPanel.SetActive(false); // DEBUG
                    yield return new WaitForSeconds((float)timeline.duration);
                    // TODO: Open UI with animation
                    speechPanel.SetActive(true); // DEBUG
                }
            }

            var textToSpeak = dialog.GetLine(currentLine, out var isPlayer);
            speech.color = isPlayer ? playerColor : dialog.npcColor;
            yield return Speak(textToSpeak);
            currentLine++;
            if (auto)
            {
                yield return new WaitForSeconds(5f);
                StartCoroutine(MainLoop());
            }  
            else
                clickAnimator.SetBool("isOn", true);
        }
        else
        {
            // TODO: Close UI with animation
            speechPanel.SetActive(false); // DEBUG
            if (dialog.postTimeline)
            {
                director.Play(dialog.postTimeline);
                yield return new WaitForSeconds((float)dialog.postTimeline.duration);
            }
            endActions.Invoke();
            enabled = false;
        }
    }

    IEnumerator Speak(string finalString)
    {
        speech.text = string.Empty;
        // TODO: Add current string to Transcript
        isSpeaking = true;
        wishToSkip = false;
        foreach (char c in finalString.Take(finalString.Length - 1))
        {
            if (wishToSkip)
                break;

            speech.text += c;
            // TODO: Play sound
            yield return new WaitForSeconds(CharTimeFor(c));
        }
        speech.text = finalString;
        isSpeaking = false;
        wishToSkip = false;
    }

    float CharTimeFor(char c) => c switch
    {
        ',' => comaTime,
        ';' => semiColonTime,
        '.' => periodTime,
        '?' => periodTime,
        '!' => periodTime,
        _ => letterTime,
    };

    /*float LetterTimeFor(string code) => code switch
    {
        "es" => .04f,
        "es-AR" => .04f,
        "en" => .05f,
        _ => letterTime,
    };*/

    void UpdateLocaleSpeed(Locale locale)
    {
        dialog.SetLanguage();
        //letterTime = LetterTimeFor(locale.Identifier.Code);
    }
}
