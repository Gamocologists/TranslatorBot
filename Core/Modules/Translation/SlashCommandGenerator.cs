using System.Collections.Generic;
using Discord;
using TranslatorBot.Data;
using TranslatorBot.Data.Translations;

namespace TranslatorBot.Modules.Translation;

/// <summary>
///     Slash command generator for the translation module.
/// </summary>
public static class SlashCommandGenerator
{

    /// <summary>
    ///     Generates the translate slash command with translates text from the automatically detected language
    ///     to another specified language.
    /// </summary>
    /// <returns>
    ///     A <see cref="SlashCommandProperties"/> object containing the translate slash command.
    /// </returns>
    public static SlashCommandProperties GenerateTranslationSlashCommand()
    {
        SlashCommandBuilder translateCommandBuilder = new ();
        translateCommandBuilder.WithName("translate");
        translateCommandBuilder.WithDescription("Translates text from the automatically detected language to another language");

        Dictionary<string, string> nameLocalizations = LocalizationHandler.GetCommandNameLocalization("translate");
        Dictionary<string, string> descriptionLocalizations = LocalizationHandler.GetCommandDescriptionLocalization("translate");

        translateCommandBuilder.WithNameLocalizations(nameLocalizations);
        translateCommandBuilder.WithDescriptionLocalizations(descriptionLocalizations);


        SlashCommandOptionBuilder targetLanguageOptionBuilder = BuildLanguageOption("translate", true);
        SlashCommandOptionBuilder textOptionBuilder = BuildTextOption();
        translateCommandBuilder.AddOption(targetLanguageOptionBuilder);
        translateCommandBuilder.AddOption(textOptionBuilder);
        
        SlashCommandProperties translateCommandProperties = translateCommandBuilder.Build();
        return translateCommandProperties;
    }
    
    /// <summary>
    ///     Generates the translate-from slash command with translates text from a specified language
    ///     to another specified language.
    /// </summary>
    /// <returns>
    ///     A <see cref="SlashCommandProperties"/> object containing the translate-from slash command.
    /// </returns>
    public static SlashCommandProperties GenerateTranslationFromSlashCommand()
    {
        SlashCommandBuilder translateCommandBuilder = new ();
        translateCommandBuilder.WithName("translate-from");
        translateCommandBuilder.WithDescription("Translates text from a specified language to another language");

        Dictionary<string, string> nameLocalizations = LocalizationHandler.GetCommandNameLocalization("translate");
        Dictionary<string, string> descriptionLocalizations = LocalizationHandler.GetCommandDescriptionLocalization("translate");

        translateCommandBuilder.WithNameLocalizations(nameLocalizations);
        translateCommandBuilder.WithDescriptionLocalizations(descriptionLocalizations);
        
        SlashCommandOptionBuilder sourceLanguageOptionBuilder = BuildLanguageOption("translate from", false);
        SlashCommandOptionBuilder targetLanguageOptionBuilder = BuildLanguageOption("translate from", true);
        SlashCommandOptionBuilder textOptionBuilder = BuildTextOption();
        translateCommandBuilder.AddOption(sourceLanguageOptionBuilder);
        translateCommandBuilder.AddOption(targetLanguageOptionBuilder);
        translateCommandBuilder.AddOption(textOptionBuilder);
        
        SlashCommandProperties translateCommandProperties = translateCommandBuilder.Build();
        return translateCommandProperties;
    }

    /// <summary>
    ///     Generates the reconnect-to-deepl slash command to attempt to reconnect to the DeepL Translation API's servers.
    /// </summary>
    /// <returns>
    ///     A <see cref="SlashCommandProperties"/> object containing the reconnect-to-deepl slash command.
    /// </returns>
    public static SlashCommandProperties GenerateReconnectToDeepLSlashCommand()
    {
        SlashCommandBuilder reconnectToDeepLCommandBuilder = new ();
        reconnectToDeepLCommandBuilder.WithName("reconnect-to-deepl");
        reconnectToDeepLCommandBuilder.WithDescription("Attempts to reconnect to the DeepL Translation API's servers");
        
        Dictionary<string, string> nameLocalizations = LocalizationHandler.GetCommandNameLocalization("reconnect to deepl");
        Dictionary<string, string> descriptionLocalizations = LocalizationHandler.GetCommandDescriptionLocalization("reconnect to deepl");
        
        reconnectToDeepLCommandBuilder.WithNameLocalizations(nameLocalizations);
        reconnectToDeepLCommandBuilder.WithDescriptionLocalizations(descriptionLocalizations);
        
        SlashCommandProperties reconnectToDeepLCommandProperties = reconnectToDeepLCommandBuilder.Build();
        return reconnectToDeepLCommandProperties;
    }

    /// <summary>
    ///     Generates the text option used in the translate slash command and the translate-from slash command.
    /// </summary>
    /// <returns>
    ///     A <see cref="SlashCommandOptionBuilder"/> object containing the text option.
    /// </returns>
    private static SlashCommandOptionBuilder BuildTextOption()
    {
        SlashCommandOptionBuilder textOptionBuilder = new ();
        textOptionBuilder.WithName("text");
        textOptionBuilder.WithDescription("The text to translate");
        textOptionBuilder.WithRequired(true);
        textOptionBuilder.WithType(ApplicationCommandOptionType.String);
        
        Dictionary<string, string> nameLocalizations = LocalizationHandler.GetCommandParameterNameLocalization("translate", "text");
        Dictionary<string, string> descriptionLocalizations = LocalizationHandler.GetCommandParameterDescriptionLocalization("translate", "text");
        
        textOptionBuilder.WithNameLocalizations(nameLocalizations);
        textOptionBuilder.WithDescriptionLocalizations(descriptionLocalizations);
        
        return textOptionBuilder;
    }

    /// <summary>
    ///      Generates the language option used in the translate slash command and the translate-from slash command.
    /// </summary>
    /// <param name="originatingCommand">
    ///      The name of the command that the language option is being generated for.
    /// </param>
    /// <param name="isTargetLanguage">
    ///      Whether the language option is for the target language or the source language.
    /// </param>
    /// <returns>
    ///      A <see cref="SlashCommandOptionBuilder"/> object containing the language option.
    /// </returns>
    private static SlashCommandOptionBuilder BuildLanguageOption(string originatingCommand, bool isTargetLanguage)
    {
        SlashCommandOptionBuilder targetLanguageOptionBuilder = new ();
        targetLanguageOptionBuilder.WithName(isTargetLanguage ? "target-language" : "source-language");
        targetLanguageOptionBuilder.WithDescription(isTargetLanguage ? "The language to translate the text to" : 
            "The language of the text to translate");
        targetLanguageOptionBuilder.WithRequired(true);
        targetLanguageOptionBuilder.WithType(ApplicationCommandOptionType.String);

        string parameterName = isTargetLanguage ? "target-language" : "source-language";
        
        Dictionary<string, string> nameLocalizations = LocalizationHandler.GetCommandParameterNameLocalization(originatingCommand, parameterName);
        Dictionary<string, string> descriptionLocalizations = LocalizationHandler.GetCommandParameterDescriptionLocalization(originatingCommand, parameterName);
        
        Dictionary<string, (string name, string value)> choices = GetChoicesForLanguages(isTargetLanguage);

        List<Choice> targetLanguageChoices = LocalizationHandler.GetCommandParameterChoicesLocalization(originatingCommand, parameterName, choices);

        targetLanguageOptionBuilder.WithNameLocalizations(nameLocalizations);
        targetLanguageOptionBuilder.WithDescriptionLocalizations(descriptionLocalizations);

        foreach (Choice choice in targetLanguageChoices)
        {
            targetLanguageOptionBuilder.AddChoice(choice);
        }

        return targetLanguageOptionBuilder;
    }

    /// <summary>
    ///     Gets the choices for the language option used in the translate slash command and the translate-from slash command.
    /// </summary>
    /// <param name="targetLanguage">
    ///     Whether the language option is for the target language or the source language.
    /// </param>
    /// <returns>
    ///     A <see cref="Dictionary{TKey,TValue}"/> object containing the choices for the language option.
    ///     The key is the name of the language, and the value is a tuple containing the name of the language and the value of the language.
    /// </returns>
    private static Dictionary<string, (string name, string value)> GetChoicesForLanguages(bool targetLanguage = true)
    {
        Dictionary<string, (string name, string value)> choices = new();

        if (!targetLanguage)
        {
            choices.Add("AutoDetected", ("Auto - Detected", "autodetected"));
        }
        
        //choices.Add("Bulgarian", ("Bulgarian", "bulgarian"));
        choices.Add("Chinese", ("Chinese", "chinese"));
        //choices.Add("Czech", ("Czech", "czech"));
        choices.Add("Danish", ("Danish", "danish"));
        choices.Add("Dutch", ("Dutch", "dutch"));
        if (targetLanguage)
        {
            choices.Add("EnglishBritish", ("English (British)", "englishgb"));
            choices.Add("EnglishAmerican", ("English (American)", "englishus"));
        }
        else
        {
            choices.Add("English", ("English", "english"));
        }
        //choices.Add("Estonian", ("Estonian", "estonian"));
        choices.Add("Finnish", ("Finnish", "finnish"));
        choices.Add("French", ("French", "french"));
        choices.Add("German", ("German", "german"));
        choices.Add("Greek", ("Greek", "greek"));
        choices.Add("Hungarian", ("Hungarian", "hungarian"));
        choices.Add("Indonesian", ("Indonesian", "indonesian"));
        choices.Add("Italian", ("Italian", "italian"));
        choices.Add("Japanese", ("Japanese", "japanese"));
        choices.Add("Korean", ("Korean", "korean"));
        //choices.Add("Latvian", ("Latvian", "latvian"));
        //choices.Add("Lithuanian", ("Lithuanian", "lithuanian"));
        //choices.Add("Norwegian", ("Norwegian", "norwegian"));
        choices.Add("Polish", ("Polish", "polish"));
        if (targetLanguage)
        {
            choices.Add("PortugueseBrazil", ("Portuguese (Brazilian)", "portuguesebz"));
            choices.Add("PortuguesePortugal", ("Portuguese (European)", "portuguesept"));
        }
        else
        {
            choices.Add("Portuguese", ("Portuguese", "portuguese"));
        }
        choices.Add("Romanian", ("Romanian", "romanian"));
        choices.Add("Russian", ("Russian", "russian"));
        //choices.Add("Slovak", ("Slovak", "slovak"));
        //choices.Add("Slovenian", ("Slovenian", "slovenian"));
        choices.Add("Spanish", ("Spanish", "spanish"));
        choices.Add("Swedish", ("Swedish", "swedish"));
        choices.Add("Turkish", ("Turkish", "turkish"));
        choices.Add("Ukrainian", ("Ukrainian", "ukrainian"));

        return choices;
    }
}