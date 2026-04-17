using UnityEngine;
using YG;

public class YGLocalizationBridge : MonoBehaviour
{
    private void OnEnable()
    {
        YG2.onSwitchLang += OnSwitchLang;
    }

    private void OnDisable()
    {
        YG2.onSwitchLang -= OnSwitchLang;
    }

    private void Start()
    {
        ApplyLanguage(YG2.lang);
    }

    private void OnSwitchLang(string lang)
    {
        ApplyLanguage(lang);
    }

    private void ApplyLanguage(string lang)
    {
        LocalisationSystem.CurrentLanguage = ConvertLanguage(lang);
    }

    private Language ConvertLanguage(string lang)
    {
        return lang switch
        {
            "ru" => Language.Russian,
            "en" => Language.English,
            _ => Language.English
        };
    }
}