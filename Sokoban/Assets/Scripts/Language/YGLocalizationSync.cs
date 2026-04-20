using UnityEngine;
using YG;
using Sokoban.GameManagement;

public class YGLocalizationSync : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        YG2.onSwitchLang += OnSwitchLang;
        YG2.onCorrectLang += OnCorrectLang;
    }

    private void OnDisable()
    {
        YG2.onSwitchLang -= OnSwitchLang;
        YG2.onCorrectLang -= OnCorrectLang;
    }

    private void Start()
    {
        ApplyLanguage(YG2.lang);
    }

    private void OnSwitchLang(string lang)
    {
        ApplyLanguage(lang);
    }

    private void OnCorrectLang(string lang)
    {
        ApplyLanguage(lang);
    }

    /*private void ApplyLanguage(string lang)
    {
        Language convertedLanguage = LanguageConverter.FromYGCode(lang);

        if (LocalisationSystem.CurrentLanguage == convertedLanguage)
            return;

        LocalisationSystem.CurrentLanguage = convertedLanguage;
        gameManager.SettingsData.CurrentLanguage = convertedLanguage;
        gameManager.SaveData();

        Debug.Log($"YG lang = {lang}, applied = {convertedLanguage}");
    }*/

    private void ApplyLanguage(string lang)
    {
        Language convertedLanguage = LanguageConverter.FromYGCode(lang);

        LocalisationSystem.CurrentLanguage = convertedLanguage;

        if (gameManager != null && gameManager.SettingsData != null)
            gameManager.SettingsData.CurrentLanguage = convertedLanguage;

        //Debug.Log($"YG lang = {lang}, applied = {convertedLanguage}");
    }
}