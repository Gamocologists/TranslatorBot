using System;

namespace TranslatorBot.Data.Translations;

public enum DiscordLanguage
{
    DANISH,
    GERMAN,
    ENGLISH_UK,
    ENGLISH_US,
    SPANISH,
    FRENCH,
    CROATIAN,
    ITALIAN,
    LITHUANIAN,
    HUNGARIAN,
    DUTCH,
    NORWEGIAN,
    POLISH,
    PORTUGUESE,
    ROMANIAN,
    FINNISH,
    SWEDISH,
    VIETNAMESE,
    TURKISH,
    CZECH,
    GREEK,
    BULGARIAN,
    RUSSIAN,
    UKRAINIAN,
    HINDI,
    THAI,
    CHINESE_CHINA,
    JAPANESE,
    CHINESE_TAIWAN,
    KOREAN
}

public static class DiscordLanguageToStringService
{
    public static string GetLanguageCode(DiscordLanguage discordLanguage)
    {
        return discordLanguage switch
        {
            DiscordLanguage.DANISH => "da",
            DiscordLanguage.GERMAN => "de",
            DiscordLanguage.ENGLISH_UK => "en-GB",
            DiscordLanguage.ENGLISH_US => "en-US",
            DiscordLanguage.SPANISH => "es-ES",
            DiscordLanguage.FRENCH => "fr",
            DiscordLanguage.CROATIAN => "hr",
            DiscordLanguage.ITALIAN => "it",
            DiscordLanguage.LITHUANIAN => "lt",
            DiscordLanguage.HUNGARIAN => "hu",
            DiscordLanguage.DUTCH => "nl",
            DiscordLanguage.NORWEGIAN => "no",
            DiscordLanguage.POLISH => "pl",
            DiscordLanguage.PORTUGUESE => "pt-BR",
            DiscordLanguage.ROMANIAN => "ro",
            DiscordLanguage.FINNISH => "fi",
            DiscordLanguage.SWEDISH => "sv-SE",
            DiscordLanguage.VIETNAMESE => "vi",
            DiscordLanguage.TURKISH => "tr",
            DiscordLanguage.CZECH => "cs",
            DiscordLanguage.GREEK => "el",
            DiscordLanguage.BULGARIAN => "bg",
            DiscordLanguage.RUSSIAN => "ru",
            DiscordLanguage.UKRAINIAN => "uk",
            DiscordLanguage.HINDI => "hi",
            DiscordLanguage.THAI => "th",
            DiscordLanguage.CHINESE_CHINA => "zh-CN",
            DiscordLanguage.JAPANESE => "ja",
            DiscordLanguage.CHINESE_TAIWAN => "zh-TW",
            DiscordLanguage.KOREAN => "ko",
            _ => throw new ArgumentOutOfRangeException(nameof(discordLanguage), discordLanguage, null)
        };
    }
    
    public static DiscordLanguage GetDiscordLanguage(string languageCode)
    {
        return languageCode switch
        {
            "da" => DiscordLanguage.DANISH,
            "de" => DiscordLanguage.GERMAN,
            "en-GB" => DiscordLanguage.ENGLISH_UK,
            "en-US" => DiscordLanguage.ENGLISH_US,
            "es-ES" => DiscordLanguage.SPANISH,
            "fr" => DiscordLanguage.FRENCH,
            "hr" => DiscordLanguage.CROATIAN,
            "it" => DiscordLanguage.ITALIAN,
            "lt" => DiscordLanguage.LITHUANIAN,
            "hu" => DiscordLanguage.HUNGARIAN,
            "nl" => DiscordLanguage.DUTCH,
            "no" => DiscordLanguage.NORWEGIAN,
            "pl" => DiscordLanguage.POLISH,
            "pt-BR" => DiscordLanguage.PORTUGUESE,
            "ro" => DiscordLanguage.ROMANIAN,
            "fi" => DiscordLanguage.FINNISH,
            "sv-SE" => DiscordLanguage.SWEDISH,
            "vi" => DiscordLanguage.VIETNAMESE,
            "tr" => DiscordLanguage.TURKISH,
            "cs" => DiscordLanguage.CZECH,
            "el" => DiscordLanguage.GREEK,
            "bg" => DiscordLanguage.BULGARIAN,
            "ru" => DiscordLanguage.RUSSIAN,
            "uk" => DiscordLanguage.UKRAINIAN,
            "hi" => DiscordLanguage.HINDI,
            "th" => DiscordLanguage.THAI,
            "zh-CN" => DiscordLanguage.CHINESE_CHINA,
            "ja" => DiscordLanguage.JAPANESE,
            "zh-TW" => DiscordLanguage.CHINESE_TAIWAN,
            "ko" => DiscordLanguage.KOREAN,
            _ => throw new ArgumentOutOfRangeException(nameof(languageCode), languageCode, null)
        };
    }
}