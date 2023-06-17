using System.Collections.Generic;
using Discord;

namespace TranslatorBot.Data;

/// <summary>
///     Represents a choice for a <see cref="Discord.ApplicationCommandOptionType" />.
///     When a command parameter has choices, the user must pick one of the choices.
/// </summary>
public class Choice
{
    /// <summary>
    ///     The name of the choice (the text that the user sees).
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    ///     The value of the choice (the value that the bot receives).
    /// </summary>
    public string Value { get; }
    
    /// <summary>
    ///     The localizations of the choice (the text that the user sees in different languages).
    /// </summary>
    public Dictionary<string, string> Localizations { get; } = GenerateDefaultLocalizations();
    
    /// <summary>
    ///     Creates a new <see cref="Choice" />.
    /// </summary>
    /// <param name="name">
    ///     The name of the choice (the text that the user sees).
    /// </param>
    /// <param name="value">
    ///     The value of the choice (the value that the bot receives).
    /// </param>
    /// <param name="localizations">
    ///     The localizations of the choice (the text that the user sees in different languages).
    /// </param>
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

    /// <summary>
    ///     Creates default localizations for a <see cref="Choice" />.
    ///     They are all the discord handled languages with an empty string as value.
    /// </summary>
    /// <returns>
    ///     A <see cref="Dictionary{TKey,TValue}" /> containing the default localizations.
    /// </returns>
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

    /// <summary>
    ///     Returns a string representation of the <see cref="Choice" />.
    /// </summary>
    /// <returns>
    ///     A string representation of the <see cref="Choice" />.
    ///     The format is: <see cref="Name" /> (<see cref="Value" />).
    /// </returns>
    public override string ToString()
    {
        return $"{Name} ({Value})";
    }
}

/// <summary>
///     Contains an extension method for <see cref="SlashCommandOptionBuilder" />.
///     This is to add a <see cref="Choice" /> to a <see cref="SlashCommandOptionBuilder" />.
/// </summary>
public static class SlashCommandOptionBuilderExtensions
{
    /// <summary>
    ///     Adds a <see cref="Choice" /> to a <see cref="SlashCommandOptionBuilder" />.
    /// </summary>
    /// <param name="builder">
    ///     The <see cref="SlashCommandOptionBuilder" /> to add the <see cref="Choice" /> to.
    /// </param>
    /// <param name="choice">
    ///     The <see cref="Choice" /> to add.
    /// </param>
    /// <returns>
    ///     The <see cref="SlashCommandOptionBuilder" /> with the added <see cref="Choice" />.
    /// </returns>
    public static SlashCommandOptionBuilder AddChoice(this SlashCommandOptionBuilder builder, Choice choice)
    {
        builder.AddChoice(choice.Name, choice.Value, choice.Localizations);
        return builder;
    }
}