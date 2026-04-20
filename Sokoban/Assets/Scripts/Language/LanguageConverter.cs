public static class LanguageConverter
{
    public static string ToYGCode(Language language)
    {
        return language switch
        {
            Language.Russian => "ru",
            Language.English => "en",
            _ => "en"
        };
    }

    public static Language FromYGCode(string lang)
    {
        if (string.IsNullOrEmpty(lang))
            return Language.English;

        return lang.ToLower() switch
        {
            "ru" => Language.Russian,
            _ => Language.English
        };
    }
}