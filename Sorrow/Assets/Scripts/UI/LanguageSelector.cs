using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine.SocialPlatforms;

public class LanguageSelector : MonoBehaviour
{
    void Awake()
    {
        List<string> languages = LocalizationSettings.AvailableLocales.Locales.Select(x => x.Identifier.CultureInfo.DisplayName).ToList();
        var dropdown = GetComponent<TMP_Dropdown>();
        dropdown.AddOptions(languages);
        dropdown.value = languages.IndexOf(LocalizationSettings.SelectedLocale.Identifier.CultureInfo.DisplayName);
    }

    public void ChangeLanguage(int value)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
        print(LocalizationSettings.SelectedLocale.Identifier.Code);
        PlayerPrefs.SetString("selected-locale", LocalizationSettings.SelectedLocale.Identifier.Code);
        PlayerPrefs.Save();
    }
}
