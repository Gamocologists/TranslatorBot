using System.Collections.Generic;
using Discord;

namespace TranslatorBot.Data;

public class Choice
{
    public string Name { get; }
    
    public string Value { get; }
    
    public Dictionary<string, string> Localizations { get; } = GenerateDefaultLocalizations();
    
    public Choice(string name, string value, Dictionary<string, string> localizations)
    {
        Name = name;
        Value = value;
        foreach (KeyValuePair<string,string> localization in localizations)
        {
            if (Localizations.ContainsKey(localization.Key))
            {
                Localizations[localization.Key] = localization.Value;
            }
        }
    }

    private static Dictionary<string,string> GenerateDefaultLocalizations()
    {
        return new()
        {
            { "da", "" },
            { "de", "" },
            { "en-GB", "" },
            { "en-US", "" },
            { "es-ES", "" },
            { "fr", "" },
            { "hr", "" },
            { "it", "" },
            { "lt", "" },
            { "hu", "" },
            { "nl", "" },
            { "no", "" },
            { "pl", "" },
            { "pt-BR", "" },
            { "ro", "" },
            { "fi", "" },
            { "sv-SE", "" },
            { "vi", "" },
            { "tr", "" },
            { "cs", "" },
            { "el", "" },
            { "bg", "" },
            { "ru", "" },
            { "uk", "" },
            { "hi", "" },
            { "th", "" },
            { "zh-CN", "" },
            { "ja", "" },
            { "zh-TW", "" },
            { "ko", "" }
        };
    }

    public override string ToString()
    {
        return $"{Name} ({Value})";
    }
}

public static class SlashCommandOptionBuilderExtensions
{
    public static SlashCommandOptionBuilder AddChoice(this SlashCommandOptionBuilder builder, Choice choice)
    {
        builder.AddChoice(choice.Name, choice.Value, choice.Localizations);
        return builder;
    }
}