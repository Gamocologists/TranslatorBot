using System.Collections.Generic;
using Discord;
using TranslatorBot.Data;
using TranslatorBot.Data.Translations;

namespace TranslatorBot.Modules.Translation;

public static class SlashCommandGenerator
{

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
    
    public static SlashCommandProperties GenerateTranslationFromSlashCommand()
    {
        SlashCommandBuilder translateCommandBuilder = new ();
        translateCommandBuilder.WithName("translate from");
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

    public static SlashCommandProperties GenerateReconnectToDeepLSlashCommand()
    {
        SlashCommandBuilder reconnectToDeepLCommandBuilder = new ();
        reconnectToDeepLCommandBuilder.WithName("reconnect to deepl");
        reconnectToDeepLCommandBuilder.WithDescription("Attempts to reconnect to the DeepL Translation API's servers");
        
        Dictionary<string, string> nameLocalizations = LocalizationHandler.GetCommandNameLocalization("reconnect to deepl");
        Dictionary<string, string> descriptionLocalizations = LocalizationHandler.GetCommandDescriptionLocalization("reconnect to deepl");
        
        reconnectToDeepLCommandBuilder.WithNameLocalizations(nameLocalizations);
        reconnectToDeepLCommandBuilder.WithDescriptionLocalizations(descriptionLocalizations);
        
        SlashCommandProperties reconnectToDeepLCommandProperties = reconnectToDeepLCommandBuilder.Build();
        return reconnectToDeepLCommandProperties;
    }

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

    private static SlashCommandOptionBuilder BuildLanguageOption(string originatingCommand, bool isTargetLanguage)
    {
        SlashCommandOptionBuilder targetLanguageOptionBuilder = new ();
        targetLanguageOptionBuilder.WithName(isTargetLanguage ? "target language" : "source language");
        targetLanguageOptionBuilder.WithDescription(isTargetLanguage ? "The language to translate the text to" : 
            "The language of the text to translate");
        targetLanguageOptionBuilder.WithRequired(true);
        targetLanguageOptionBuilder.WithType(ApplicationCommandOptionType.String);

        string parameterName = isTargetLanguage ? "target language" : "source language";
        
        Dictionary<string, string> nameLocalizations = LocalizationHandler.GetCommandParameterNameLocalization(originatingCommand, parameterName);
        Dictionary<string, string> descriptionLocalizations = LocalizationHandler.GetCommandParameterDescriptionLocalization(originatingCommand, parameterName);
        
        Dictionary<string, (string name, int value)> choices = GetChoicesForLanguages(isTargetLanguage);

        List<Choice> targetLanguageChoices = LocalizationHandler.GetCommandParameterChoicesLocalization(originatingCommand, parameterName, choices);

        targetLanguageOptionBuilder.WithNameLocalizations(nameLocalizations);
        targetLanguageOptionBuilder.WithDescriptionLocalizations(descriptionLocalizations);

        foreach (Choice choice in targetLanguageChoices)
        {
            targetLanguageOptionBuilder.AddChoice(choice);
        }

        return targetLanguageOptionBuilder;
    }

    private static Dictionary<string, (string name, int value)> GetChoicesForLanguages(bool targetLanguage = true)
    {
        Dictionary<string, (string name, int value)> choices = new();

        int offset = 0;
        
        if (!targetLanguage)
        {
            choices.Add("AutoDetected", ("Auto - Detected", 0));
        }
        
        choices.Add("Bulgarian", ("Bulgarian", 1));
        choices.Add("Chinese", ("Chinese", 2));
        choices.Add("Czech", ("Czech", 3));
        choices.Add("Danish", ("Danish", 4));
        choices.Add("Dutch", ("Dutch", 5));
        if (targetLanguage)
        {
            choices.Add("EnglishBritish", ("English (British)", 6));
            choices.Add("EnglishAmerican", ("English (American)", 7));
        }
        else
        {
            choices.Add("English", ("English", 6));
            offset = 1;
        }
        choices.Add("Estonian", ("Estonian", 8 - offset));
        choices.Add("Finnish", ("Finnish", 9 - offset));
        choices.Add("French", ("French", 10 - offset));
        choices.Add("German", ("German", 11 - offset));
        choices.Add("Greek", ("Greek", 12 - offset));
        choices.Add("Hungarian", ("Hungarian", 13 - offset));
        choices.Add("Indonesian", ("Indonesian", 14 - offset));
        choices.Add("Italian", ("Italian", 15 - offset));
        choices.Add("Japanese", ("Japanese", 16 - offset));
        choices.Add("Korean", ("Korean", 17 - offset));
        choices.Add("Latvian", ("Latvian", 18 - offset));
        choices.Add("Lithuanian", ("Lithuanian", 19 - offset));
        choices.Add("Norwegian", ("Norwegian", 20 - offset));
        choices.Add("Polish", ("Polish", 21 - offset));
        if (targetLanguage)
        {
            choices.Add("PortugueseBrazil", ("Portuguese (Brazilian)", 22));
            choices.Add("PortuguesePortugal", ("Portuguese (European)", 23));
        }
        else
        {
            choices.Add("Portuguese", ("Portuguese", 22));
            offset = 2;
        }
        choices.Add("Romanian", ("Romanian", 24 - offset));
        choices.Add("Russian", ("Russian", 25 - offset));
        choices.Add("Slovak", ("Slovak", 26 - offset));
        choices.Add("Slovenian", ("Slovenian", 27 - offset));
        choices.Add("Spanish", ("Spanish", 28 - offset));
        choices.Add("Swedish", ("Swedish", 29 - offset));
        choices.Add("Turkish", ("Turkish", 30 - offset));
        choices.Add("Ukrainian", ("Ukrainian", 31 - offset));

        return choices;
    }
}