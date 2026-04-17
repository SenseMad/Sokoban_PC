using Sokoban.GameManagement;

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
        return lang switch
        {
            "ru" => Language.Russian,
            "en" => Language.English,
            _ => Language.English
        };
    }
}