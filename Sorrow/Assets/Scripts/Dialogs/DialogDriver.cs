using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Playables;
using TMPro;

public class DialogDriver : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TMP_Text speech;

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
        director = GetComponent<PlayableDirector>();
        letterTime = LetterTimeFor(LocalizationSettings.SelectedLocale.Identifier.Code);
        LocalizationSettings.SelectedLocaleChanged += UpdateLocaleSpeed;
        InputManager.controller.Dialog.Auto.performed += SetAuto;
        InputManager.controller.Dialog.Continue.performed += Continue;
        // Suscribir abrir transcript
    }

    void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= UpdateLocaleSpeed;
        InputManager.controller.Dialog.Auto.performed -= SetAuto;
        InputManager.controller.Dialog.Continue.performed -= Continue;
        // Desuscribir abrir transcript
    }

    void Start() => StartCoroutine(StartingCoroutine());

    IEnumerator StartingCoroutine()
    {
        if (dialog.preTimeline)
        {
            director.Play(dialog.preTimeline);
            yield return new WaitForSeconds((float)dialog.preTimeline.duration);
        }
        // Open UI
        yield return MainLoop();
    }

    void SetAuto(InputAction.CallbackContext _)
    {
        if (!auto)
            StartCoroutine(AutoMode());

        auto ^= true;
    }

    void Continue(InputAction.CallbackContext _)
    {
        if (isSpeaking)
            wishToSkip = true;
        else if (director.state != PlayState.Playing)
            StartCoroutine(MainLoop());
    }

    IEnumerator AutoMode()
    {
        while (isSpeaking)
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
                    yield return new WaitForSeconds((float)timeline.duration);
                wishToSkip = false;
            }
            // Set speaker color

            yield return Speak(dialog.GetLine(currentLine));
            currentLine++;
        }
        else
        {
            director.Play(dialog.postTimeline);
            yield return new WaitForSeconds((float)dialog.postTimeline.duration);
            Destroy(this);
        }
    }

    IEnumerator Speak(string finalString)
    {
        speech.text = string.Empty;
        // Add current string to Transcript
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
            // Play sound
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
        => letterTime = LetterTimeFor(locale.Identifier.Code);
}
