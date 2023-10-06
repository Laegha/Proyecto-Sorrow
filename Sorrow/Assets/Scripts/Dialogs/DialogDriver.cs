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

public class DialogDriver : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject speechPanel;
    [SerializeField] GameObject transcriptPanel;
    TMP_Text speech;
    ScrollRect transcript;

    [Header("Dialog")]
    [SerializeField] Dialog dialog;
    [SerializeField] Color playerColor;
    [SerializeField] float comaTime = 1f;
    [SerializeField] float semiColonTime = 1.5f;
    [SerializeField] float periodTime = 2f;

    int currentLine = 0;
    PlayableDirector director;
    float letterTime = 0.1f;
    bool wishToSkip = false;
    bool isSpeaking = false;
    bool auto = false;

    void Awake()
    {
        dialog.SetLanguage();
        director = GetComponent<PlayableDirector>();
        letterTime = LetterTimeFor(LocalizationSettings.SelectedLocale.Identifier.Code);
        speech = speechPanel.GetComponentInChildren<TMP_Text>();
        transcript = transcriptPanel.GetComponentInChildren<ScrollRect>();
        LocalizationSettings.SelectedLocaleChanged += UpdateLocaleSpeed;
    }

    void OnDisable()
    {
        InputManager.controller.Dialog.Auto.performed -= SetAuto;
        InputManager.controller.Dialog.Continue.performed -= Continue;
        InputManager.controller.Dialog.ShowTranscript.performed -= OpenTranscript;
        InputManager.controller.Dialog.Disable();
        InputManager.controller.Movement.Enable();
        InputManager.controller.Camera.Enable();
    }

    void OnEnable()
    {
        InputManager.controller.Camera.Disable();
        InputManager.controller.Movement.Disable();
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
        if (!auto)
            StartCoroutine(AutoMode());

        auto ^= true;
        // TODO: Change icon
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

    IEnumerator AutoMode()
    {
        while (isSpeaking || director.state == PlayState.Playing)
            yield return new WaitForSeconds(periodTime);
        while (auto)
        {
            yield return MainLoop();
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator MainLoop()
    {
        if (currentLine < dialog.Count)
        {
            if (dialog.TryInterrupt(currentLine, out var timeline, out var dontStopText))
            {
                director.Play(timeline);
                if (!dontStopText)
                {
                    // TODO: Close UI with animation
                    speechPanel.SetActive(false); // DEBUG
                    yield return new WaitForSeconds((float)timeline.duration);
                    // TODO: Open UI with animation
                    speechPanel.SetActive(true); // DEBUG
                }
                wishToSkip = false;
            }

            var textToSpeak = dialog.GetLine(currentLine, out var isPlayer);
            speech.color = isPlayer ? playerColor : dialog.npcColor;
            yield return Speak(textToSpeak);
            currentLine++;
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
            enabled = false;
        }
    }

    IEnumerator Speak(string finalString)
    {
        speech.text = string.Empty;
        // TODO: Add current string to Transcript
        isSpeaking = true;
        foreach (char c in finalString)
        {
            if (wishToSkip)
            {
                speech.text = finalString;
                yield return new WaitForSeconds(1f);
                break;
            }
            speech.text += c;
            // TODO: Play sound
            yield return new WaitForSeconds(CharTimeFor(c));
        }
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

    float LetterTimeFor(string code) => code switch
    {
        "es" => 0.07f,
        "es-AR" => 0.07f,
        "en" => 0.09f,
        _ => letterTime,
    };

    void UpdateLocaleSpeed(Locale locale)
    {
        dialog.SetLanguage();
        letterTime = LetterTimeFor(locale.Identifier.Code);
    }
}
