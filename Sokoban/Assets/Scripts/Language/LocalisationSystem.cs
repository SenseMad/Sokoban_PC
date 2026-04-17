using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LocalisationSystem : SingletonInSceneNoInstance<LocalisationSystem>
{
    private static Language _currentLanguage = Language.English;

    private static Dictionary<string, string> localisedRU;
    private static Dictionary<string, string> localisedEN;
    /*private static Dictionary<string, string> localisedSP;
    private static Dictionary<string, string> localisedPO;

    private static Dictionary<string, string> localisedFR;
    private static Dictionary<string, string> localisedJA;
    private static Dictionary<string, string> localisedGE;
    private static Dictionary<string, string> localisedCH;*/

    public static bool IsInit;

    //======================================

    [SerializeField]
    private FontAsset _fontAsset;

    //======================================

    /*public static Language CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            _currentLanguage = value;
            ChangeCurrentLanguage?.Invoke(value);
        }
    }*/

    public static Language CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            Language validatedLanguage = ValidateLanguage(value);

            if (_currentLanguage == validatedLanguage)
                return;

            _currentLanguage = validatedLanguage;
            ChangeCurrentLanguage?.Invoke(_currentLanguage);
        }
    }


    //======================================

    public static event Action<Language> ChangeCurrentLanguage;

    //======================================

    private static readonly Language[] _enabledLanguages =
    {
        Language.Russian,
        Language.English
    };

    /*public TMP_FontAsset GetFont()
    {
        return _fontAsset.GetFont(_currentLanguage);
    }

    /// <summary>
    /// Получить шрифт локализации
    /// </summary>
    public TMP_FontAsset GetLocalizationFont(Language parLanguage)
    {
        return _fontAsset.GetFont(parLanguage);
    }*/

    /*public void Init()
    {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        localisedRU = csvLoader.GetDictionaryValues("ru");
        localisedEN = csvLoader.GetDictionaryValues("en");
        localisedFR = csvLoader.GetDictionaryValues("fr");
        localisedJA = csvLoader.GetDictionaryValues("ja");
        localisedGE = csvLoader.GetDictionaryValues("ge");
        localisedSP = csvLoader.GetDictionaryValues("sp");
        localisedPO = csvLoader.GetDictionaryValues("po");
        localisedCH = csvLoader.GetDictionaryValues("ch");

        IsInit = true;
    }*/

    public TMP_FontAsset GetFont()
    {
        return _fontAsset.GetFont(CurrentLanguage);
    }

    public TMP_FontAsset GetLocalizationFont(Language parLanguage)
    {
        return _fontAsset.GetFont(ValidateLanguage(parLanguage));
    }

    public void Init()
    {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        localisedRU = csvLoader.GetDictionaryValues("ru");
        localisedEN = csvLoader.GetDictionaryValues("en");
        /*localisedSP = csvLoader.GetDictionaryValues("sp");
        localisedPO = csvLoader.GetDictionaryValues("po");*/

        IsInit = true;
    }

    public string GetLocalisedValue(string key)
    {
        if (!IsInit)
            Init();

        string value = key;

        switch (ValidateLanguage(CurrentLanguage))
        {
            case Language.Russian:
                if (localisedRU != null && localisedRU.TryGetValue(key, out value))
                    return value;
                break;

            case Language.English:
                if (localisedEN != null && localisedEN.TryGetValue(key, out value))
                    return value;
                break;

            /*case Language.Spanish:
                if (localisedSP != null && localisedSP.TryGetValue(key, out value))
                    return value;
                break;

            case Language.Portuguese:
                if (localisedPO != null && localisedPO.TryGetValue(key, out value))
                    return value;
                break;*/
        }

        if (localisedEN != null && localisedEN.TryGetValue(key, out string fallbackValue))
            return fallbackValue;

        return key;
    }

    /*public string GetLocalisedValue(string key)
    {
        if (!IsInit) { Init(); }

        string value = key;

        switch (CurrentLanguage)
        {
            case Language.Russian:
                localisedRU.TryGetValue(key, out value);
                break;
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
            case Language.French:
                localisedFR.TryGetValue(key, out value);
                break;
            case Language.Japan:
                localisedJA.TryGetValue(key, out value);
                break;
            case Language.German:
                localisedGE.TryGetValue(key, out value);
                break;
            case Language.Spanish:
                localisedSP.TryGetValue(key, out value);
                break;
            case Language.Portuguese:
                localisedPO.TryGetValue(key, out value);
                break;
            case Language.Chinese:
                localisedCH.TryGetValue(key, out value);
                break;
        }

        return value;
    }*/

    public static Language[] GetNamesAllLanguage()
    {
        return _enabledLanguages;
    }

    /*public static Language[] GetNamesAllLanguage()
    {
        return (Language[])Enum.GetValues(typeof(Language));
    }*/

    /*public static string GetNameLanguage(Language parLanguage)
    {
        return parLanguage switch
        {
            Language.Russian => "Русский",
            Language.English => "English",
            Language.French => "Français",
            Language.Japan => "日本の",
            Language.German => "Deutsch",
            Language.Spanish => "Español",
            Language.Portuguese => "Português",
            Language.Chinese => "中国人",
            _ => "English",
        };
    }*/

    public static string GetNameLanguage(Language parLanguage)
    {
        return ValidateLanguage(parLanguage) switch
        {
            Language.Russian => "Русский",
            Language.English => "English",
            /*Language.Spanish => "Español",
            Language.Portuguese => "Português",*/
            _ => "English"
        };
    }

    private static Language ValidateLanguage(Language language)
    {
        for (int i = 0; i < _enabledLanguages.Length; i++)
        {
            if (_enabledLanguages[i] == language)
                return language;
        }

        return Language.English;
    }

    //======================================
}