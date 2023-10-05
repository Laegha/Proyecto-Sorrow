using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Playables;

public class DialogDriver : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] Color playerColor;
    [SerializeField] float comaTime = 1f;
    [SerializeField] float semiColonTime = 2f;
    [SerializeField] float periodTime = 4f;
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
        // Set InputManager Inputs
    }

    void Start() => StartCoroutine(MainLoop());

    IEnumerator AutoMode()
    {
        while (currentLine < dialog.Count)
            yield return MainLoop();
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
            }

            // Set speaker color

            yield return Speak(dialog.GetLine(currentLine));
            currentLine++;
        }
    }

    IEnumerator Speak(string finalString)
    {
        // Clear UI string
        // Add current string to Transcript
        isSpeaking = true;
        foreach (char c in finalString)
        {
            if (wishToSkip)
            {
                // Set UI string == finalString
                yield return new WaitForSeconds(1f);
                break;
            }
            // Add character to UI string
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

    void UpdateLocaleSpeed(Locale locale) => letterTime = LetterTimeFor(locale.Identifier.Code);
}
