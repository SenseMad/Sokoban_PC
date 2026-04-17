using UnityEngine;
using YG;

public static class CorrectLang
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        YG2.onCorrectLang += OnCorrectLang;
    }

    private static void OnCorrectLang(string lang)
    {
        if (lang != "ru" && lang != "en")
            YG2.lang = "en";
    }
}