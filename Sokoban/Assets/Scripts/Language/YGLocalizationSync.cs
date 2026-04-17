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
        Language convertedLanguage = LanguageConverter.FromYGCode(lang);

        LocalisationSystem.CurrentLanguage = convertedLanguage;
        gameManager.SettingsData.CurrentLanguage = convertedLanguage;
        gameManager.SaveData();
    }
}