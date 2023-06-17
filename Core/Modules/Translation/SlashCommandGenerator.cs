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

    private static Dictionary<string, (string name, string value)> GetChoicesForLanguages(bool targetLanguage = true)
    {
        Dictionary<string, (string name, string value)> choices = new();

        int offset = 0;
        
        if (!targetLanguage)
        {
            choices.Add("AutoDetected", ("Auto - Detected", "autodetected"));
        }
        
        //choices.Add("Bulgarian", ("Bulgarian", "bulgarian"));
        offset += 1;
        choices.Add("Chinese", ("Chinese", "chinese"));
        //choices.Add("Czech", ("Czech", "czech"));
        offset += 1;
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
            offset += 1;
        }
        //choices.Add("Estonian", ("Estonian", "estonian"));
        offset += 1;
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
        offset += 3;
        choices.Add("Polish", ("Polish", "polish"));
        if (targetLanguage)
        {
            choices.Add("PortugueseBrazil", ("Portuguese (Brazilian)", "portuguesebz"));
            choices.Add("PortuguesePortugal", ("Portuguese (European)", "portuguesept"));
        }
        else
        {
            choices.Add("Portuguese", ("Portuguese", "portuguese"));
            offset += 1;
        }
        choices.Add("Romanian", ("Romanian", "romanian"));
        choices.Add("Russian", ("Russian", "russian"));
        //choices.Add("Slovak", ("Slovak", "slovak"));
        //choices.Add("Slovenian", ("Slovenian", "slovenian"));
        offset += 2;
        choices.Add("Spanish", ("Spanish", "spanish"));
        choices.Add("Swedish", ("Swedish", "swedish"));
        choices.Add("Turkish", ("Turkish", "turkish"));
        choices.Add("Ukrainian", ("Ukrainian", "ukrainian"));

        return choices;
    }
}