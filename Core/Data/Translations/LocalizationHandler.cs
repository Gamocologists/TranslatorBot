using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TranslatorBot.Data.Translations;

/// <summary>
///     Handles localization data.
/// </summary>
public static class LocalizationHandler
{
    /// <summary>
    ///     The base path for all localization files (the root of the localization directory).
    /// </summary>
    public static readonly string BasePath = ComputeBasePath();

    /// <summary>
    ///     Computes the base path for all localization files (the root of the localization directory).
    /// </summary>
    /// <returns>
    ///     The base path for all localization files (the root of the localization directory).
    /// </returns>
    /// <exception cref="FileNotFoundException">
    ///     Thrown when the TranslatorBot directory could not be found.
    /// </exception>
    private static string ComputeBasePath()
    {
        string basePath = AppContext.BaseDirectory;
        
        while (!basePath.EndsWith("TranslatorBot"))
        {
            DirectoryInfo? directoryInfo = Directory.GetParent(basePath);
            if (directoryInfo is null)
            {
                throw new FileNotFoundException("Could not find TranslatorBot directory");
            }
            
            basePath = directoryInfo.FullName;
        }
        
        basePath = Path.Combine(basePath, "Core", "Data", "Translations");
        return basePath;
    }
    
    /// <summary>
    ///     Gets the path to the root of the localization directory for a command.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="createIfNotFound">
    ///     Whether the directory should be created if it does not exist. By default, it does not.
    /// </param>
    /// <returns>
    ///     The path to the root of the localization directory for a command.
    /// </returns>
    public static string GetCommandLocalizationPath(string commandName, bool createIfNotFound = false)
    {
        string path = Path.Combine(BasePath, "COMMANDS", commandName);
        if (createIfNotFound && !Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        return path;
    }
    
    /// <summary>
    ///     Gets the path to the root of the localization directory for a command parameter.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter.
    /// </param>
    /// <param name="createIfNotFound">
    ///     Whether the directory should be created if it does not exist. By default, it does not.
    /// </param>
    /// <returns>
    ///     The path to the root of the localization directory for a command parameter.
    /// </returns>
    public static string GetCommandParameterLocalizationPath(string commandName, string parameter, bool createIfNotFound = false)
    {
        string path = Path.Combine(BasePath, "COMMANDS", commandName, "PARAMETERS", parameter);
        if (createIfNotFound && !Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        return path;
    }
    
    /// <summary>
    ///     Gets the dictionary containing the localization data for a command's name.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="createIfNotFound">
    ///     Whether the file should be created if it does not exist. By default, it does not.
    /// </param>
    /// <returns>
    ///     The dictionary containing the localization data for a command's name.
    ///     If the file does not exist, an empty dictionary is returned.
    ///     The dictionary is keyed by the language code, and the value is the translation.
    /// </returns>
    public static Dictionary<string, string> GetCommandNameLocalization(string commandName, bool createIfNotFound = false)
    {
        string path = GetCommandLocalizationPath(commandName, createIfNotFound);
        
        string namePath = Path.Combine(path, "names.json");
        
        return RetrieveLocalizationAtPath(namePath);
    }
    
    /// <summary>
    ///     Gets the dictionary containing the localization data for a command's description.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="createIfNotFound">
    ///     Whether the file should be created if it does not exist. By default, it does not.
    /// </param>
    /// <returns>
    ///     The dictionary containing the localization data for a command's description.
    ///     If the file does not exist, an empty dictionary is returned.
    ///     The dictionary is keyed by the language code, and the value is the translation.
    /// </returns>
    public static Dictionary<string, string> GetCommandDescriptionLocalization(string commandName, bool createIfNotFound = false)
    {
        string path = GetCommandLocalizationPath(commandName, createIfNotFound);
        
        string descriptionPath = Path.Combine(path, "descriptions.json");
        
        return RetrieveLocalizationAtPath(descriptionPath);
    }
    
    /// <summary>
    ///     Gets the dictionary containing the localization data for a command parameter's name.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter.
    /// </param>
    /// <param name="createIfNotFound">
    ///     Whether the file should be created if it does not exist. By default, it does not.
    /// </param>
    /// <returns>
    ///     The dictionary containing the localization data for a command parameter's name.
    ///     If the file does not exist, an empty dictionary is returned.
    ///     The dictionary is keyed by the language code, and the value is the translation.
    /// </returns>
    public static Dictionary<string, string> GetCommandParameterNameLocalization(string commandName, string parameter, bool createIfNotFound = false)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, createIfNotFound);
        
        string namePath = Path.Combine(path, "names.json");
        
        return RetrieveLocalizationAtPath(namePath);
    }
    
    /// <summary>
    ///     Gets the dictionary containing the localization data for a command parameter's description.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter.
    /// </param>
    /// <param name="createIfNotFound">
    ///     Whether the file should be created if it does not exist. By default, it does not.
    /// </param>
    /// <returns>
    ///     The dictionary containing the localization data for a command parameter's description.
    ///     If the file does not exist, an empty dictionary is returned.
    ///     The dictionary is keyed by the language code, and the value is the translation.
    /// </returns>
    public static Dictionary<string, string> GetCommandParameterDescriptionLocalization(string commandName, string parameter, bool createIfNotFound = false)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, createIfNotFound);
        
        string descriptionPath = Path.Combine(path, "descriptions.json");
        
        return RetrieveLocalizationAtPath(descriptionPath);
    }

    /// <summary>
    ///     Helper method to retrieve a localization file at a given path.
    ///     If the file does not exist, an empty dictionary is returned.
    /// </summary>
    /// <param name="namePath">
    ///     The path to the localization file.
    /// </param>
    /// <returns>
    ///     The dictionary containing the localization data.
    ///     If the file does not exist, an empty dictionary is returned.
    /// </returns>
    private static Dictionary<string, string> RetrieveLocalizationAtPath(string namePath)
    {
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        using StreamReader writer = new(namePath);
        string jsonText = writer.ReadToEnd();
        writer.Close();

        Dictionary<string, string>? nameLocalizations =
            JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText, options);
        return nameLocalizations ?? new Dictionary<string, string>();
    }
    
    /// <summary>
    ///     Adds or sets a localization entry for a command's name.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="discordLanguage">
    ///     The language code of the translation.
    /// </param>
    /// <param name="translation">
    ///     The translation.
    /// </param>
    /// <returns>
    ///     The previous translation, if it existed.
    /// </returns>
    public static string AddOrSetCommandNameLocalization(string commandName, DiscordLanguage discordLanguage, string translation)
    {
        string path = GetCommandLocalizationPath(commandName, true);
        
        return AddOrSetLocalizationEntry(Path.Combine(path, "names.json"), discordLanguage, translation);
    }
    
    /// <summary>
    ///     Adds or sets a localization entry for a command's description.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="discordLanguage">
    ///     The language code of the translation.
    /// </param>
    /// <param name="translation">
    ///     The translation.
    /// </param>
    /// <returns>
    ///     The previous translation, if it existed.
    /// </returns>
    public static string AddOrSetCommandDescriptionLocalization(string commandName, DiscordLanguage discordLanguage, string translation)
    {
        string path = GetCommandLocalizationPath(commandName, true);
        
        return AddOrSetLocalizationEntry(Path.Combine(path, "descriptions.json"), discordLanguage, translation);
    }
    
    /// <summary>
    ///     Adds or sets a localization entry for a command parameter's name.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter.
    /// </param>
    /// <param name="discordLanguage">
    ///     The language code of the translation.
    /// </param>
    /// <param name="translation">
    ///     The translation.
    /// </param>
    /// <returns>
    ///     The previous translation, if it existed.
    /// </returns>
    public static string AddOrSetCommandParameterNameLocalization(string commandName, string parameter, DiscordLanguage discordLanguage, string translation)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, true);
        
        return AddOrSetLocalizationEntry(Path.Combine(path, "names.json"), discordLanguage, translation);
    }
    
    /// <summary>
    ///     Adds or sets a localization entry for a command parameter's description.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter.
    /// </param>
    /// <param name="discordLanguage">
    ///     The language code of the translation.
    /// </param>
    /// <param name="translation">
    ///     The translation.
    /// </param>
    /// <returns>
    ///     The previous translation, if it existed.
    /// </returns>
    public static string AddOrSetCommandParameterDescriptionLocalization(string commandName, string parameter, DiscordLanguage discordLanguage, string translation)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, true);
        
        return AddOrSetLocalizationEntry(Path.Combine(path, "descriptions.json"), discordLanguage, translation);
    }
    
    /// <summary>
    ///     Helper method to add or set a localization entry at a given path.
    ///     If the file does not exist, it is created.
    /// </summary>
    /// <param name="path">
    ///     The path to the localization file.
    /// </param>
    /// <param name="discordLanguage">
    ///     The language code of the translation.
    /// </param>
    /// <param name="translation">
    ///     The translation.
    /// </param>
    /// <returns>
    ///     The previous translation, if it existed.
    /// </returns>
    public static string AddOrSetLocalizationEntry(string path, DiscordLanguage discordLanguage, string translation)
    {
        Dictionary<string, string> localizations = RetrieveLocalizationAtPath(path);

        string languageCode = DiscordLanguageToStringService.GetLanguageCode(discordLanguage);
        
        string oldTranslation;
        if (localizations.ContainsKey(languageCode))
        {
            oldTranslation = localizations[languageCode];
            localizations[languageCode] = translation;
        }
        else
        {
            oldTranslation = "";
            localizations.Add(languageCode, translation);
        }

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        
        string jsonText = JsonSerializer.Serialize(localizations, options);
        
        using StreamWriter writer = new(path);
        writer.Write(jsonText);
        writer.Close();
        
        return oldTranslation;
    }

    /// <summary>
    ///     Removes a localization entry at a given path.
    /// </summary>
    /// <param name="path">
    ///     The path to the localization file.
    /// </param>
    /// <param name="discordLanguage">
    ///     The language code of the translation.
    /// </param>
    /// <returns>
    ///     True if the entry was removed, false if it did not exist.
    /// </returns>
    public static bool RemoveLocalizationEntry(string path, DiscordLanguage discordLanguage)
    {
        Dictionary<string, string> localizations = RetrieveLocalizationAtPath(path);
        
        string languageCode = DiscordLanguageToStringService.GetLanguageCode(discordLanguage);
        
        bool wasRemoved = localizations.Remove(languageCode);
        
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        
        string jsonText = JsonSerializer.Serialize(localizations, options);
        using StreamWriter writer = new(path);
        writer.Write(jsonText);
        
        return wasRemoved;
    }
    
    /// <summary>
    ///     Generates boilerplate localization data for all commands.
    /// </summary>
    /// <param name="overwriteExisting">
    ///     Whether or not to overwrite existing localization data. By default, this is false.
    /// </param>
    public static void GenerateBoilerplateLocalizationData(bool overwriteExisting = false)
    {
        List<string> commandNames = Commands.GetCommandNames();
        
        foreach (string commandName in commandNames)
        {
            GenerateBoilerplateCommandLocalizationData(commandName, overwriteExisting);
        }
        
    }

    /// <summary>
    ///     Helper method to generate boilerplate localization data for a given command.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="overwriteExisting">
    ///     Whether or not to overwrite existing localization data. By default, this is false.
    /// </param>
    private static void GenerateBoilerplateCommandLocalizationData(string commandName, bool overwriteExisting)
    {
        string path = GetCommandLocalizationPath(commandName, true);
        
        GenerateBoilerplateCommandNameLocalizationData(path, overwriteExisting);
        GenerateBoilerplateCommandDescriptionLocalizationData(path, overwriteExisting);
        
        List<string> parameterNames = Commands.GetCommandParameterNames(commandName, true);
        
        foreach (string parameterName in parameterNames)
        {
            GenerateBoilerplateCommandParameterNameLocalizationData(commandName, parameterName, overwriteExisting);
            GenerateBoilerplateCommandParameterDescriptionLocalizationData(commandName, parameterName, overwriteExisting);
        }
    }

    /// <summary>
    ///     Helper method to generate boilerplate localization data for a given command's name.
    /// </summary>
    /// <param name="path">
    ///     The path to the command's localization data.
    /// </param>
    /// <param name="overwriteExisting">
    ///     Whether or not to overwrite existing localization data. By default, this is false.
    /// </param>
    private static void GenerateBoilerplateCommandNameLocalizationData(string path, bool overwriteExisting)
    {
        string namePath = Path.Combine(path, "names.json");
        
        GenerateBoilerplateLocalizationData(namePath, overwriteExisting);
    }
    
    /// <summary>
    ///     Helper method to generate boilerplate localization data for a given command's description.
    /// </summary>
    /// <param name="path">
    ///     The path to the command's localization data.
    /// </param>
    /// <param name="overwriteExisting">
    ///     Whether or not to overwrite existing localization data. By default, this is false.
    /// </param>
    private static void GenerateBoilerplateCommandDescriptionLocalizationData(string path, bool overwriteExisting)
    {
        string descriptionPath = Path.Combine(path, "descriptions.json");
        
        GenerateBoilerplateLocalizationData(descriptionPath, overwriteExisting);
    }
    
    /// <summary>
    ///     Helper method to generate boilerplate localization data for a given command's parameter name.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter.
    /// </param>
    /// <param name="overwriteExisting">
    ///     Whether or not to overwrite existing localization data. By default, this is false.
    /// </param>
    private static void GenerateBoilerplateCommandParameterNameLocalizationData(string commandName, string parameter, bool overwriteExisting)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, true);
        
        string namePath = Path.Combine(path, "names.json");
        
        GenerateBoilerplateLocalizationData(namePath, overwriteExisting);
    }
    
    /// <summary>
    ///     Helper method to generate boilerplate localization data for a given command's parameter description.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter.
    /// </param>
    /// <param name="overwriteExisting">
    ///     Whether or not to overwrite existing localization data. By default, this is false.
    /// </param>
    private static void GenerateBoilerplateCommandParameterDescriptionLocalizationData(string commandName, string parameter, bool overwriteExisting)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, true);
        
        string descriptionPath = Path.Combine(path, "descriptions.json");
        
        GenerateBoilerplateLocalizationData(descriptionPath, overwriteExisting);
    }

    /// <summary>
    ///     Helper method to generate boilerplate localization data for a given path.
    /// </summary>
    /// <param name="path">
    ///     The path to the localization data.
    /// </param>
    /// <param name="overwriteExisting">
    ///     Whether or not to overwrite existing localization data. By default, this is false.
    /// </param>
    private static void GenerateBoilerplateLocalizationData(string path, bool overwriteExisting)
    {
        if (!overwriteExisting && File.Exists(path))
        {
            return;
        }
        
        using StreamWriter writer = new(path);
        
        Dictionary<string, string> boilerplateData = new()
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
        
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        
        string jsonText = JsonSerializer.Serialize(boilerplateData, options);
        writer.Write(jsonText);
        writer.Close();
    }

    /// <summary>
    ///     Retrieves the localization data for a given command parameter's choices.
    ///     Only the choices for which data has been provided will be returned.
    /// </summary>
    /// <param name="command">
    ///     The name of the command that the localization data is for.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter that the localization data is for.
    /// </param>
    /// <param name="choices">
    ///     The choices that the localization data is for.
    /// </param>
    /// <returns>
    ///     A list of choices with their localization data.
    /// </returns>
    public static List<Choice> GetCommandParameterChoicesLocalization(string command, string parameter,
        Dictionary<string, (string name, string value)> choices)
    {
        List<Choice> results = new();
        
        foreach (KeyValuePair<string, (string name, string value)> choice in choices)
        {
            string path = GetCommandParameterChoiceLocalizationPath(command, parameter, choice.Key, true);
            
            Dictionary<string, string> localization = RetrieveLocalizationAtPath(path);
            
            string name = choice.Value.name;
            string value = choice.Value.value;
            
            // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
            if (localization.ContainsKey("en-US"))
            {
                name = localization["en-US"];
            }
            
            Choice newChoice = new(name, value, localization);
            results.Add(newChoice);
        }
        
        return results;
    }

    /// <summary>
    ///     Helper method to retrieve the path to a given command parameter's choice localization data.
    /// </summary>
    /// <param name="command">
    ///     The name of the command that the localization data is for.
    /// </param>
    /// <param name="parameter">
    ///     The name of the parameter that the localization data is for.
    /// </param>
    /// <param name="choice">
    ///     The name of the choice that the localization data is for.
    /// </param>
    /// <param name="createIfNotExists">
    ///     Whether or not to create the path if it does not exist. By default, this is false.
    /// </param>
    /// <returns>
    ///     The path to the localization data.
    /// </returns>
    private static string GetCommandParameterChoiceLocalizationPath(string command, string parameter, string choice, bool createIfNotExists)
    {
        string path = GetCommandParameterLocalizationPath(command, parameter, createIfNotExists);
        
        return Path.Combine(path, "OPTIONS", choice, "translations.json");
    }

    /// <summary>
    ///     Helper method to retrieve the path to a given embed's localization data directory.
    /// </summary>
    /// <param name="embedName">
    ///     The name of the embed that the localization data is for.
    /// </param>
    /// <returns>
    ///     The path to the localization data directory.
    /// </returns>
    private static string GetEmbedPath(string embedName)
    {
        return Path.Combine(BasePath, "EMBEDS", embedName);
    }
    
    /// <summary>
    ///     Helper method to retrieve the path to a given embed's field title localization data.
    /// </summary>
    /// <param name="embedName">
    ///     The name of the embed that the localization data is for.
    /// </param>
    /// <returns>
    ///     The path to the localization data.
    /// </returns>
    private static string GetEmbedFieldTitlePath(string embedName)
    {
        return Path.Combine(GetEmbedPath(embedName), "field_title.json");
    }
    
    /// <summary>
    ///     Helper method to retrieve the path to a given embed's field value localization data.
    /// </summary>
    /// <param name="embedName">
    ///     The name of the embed that the localization data is for.
    /// </param>
    /// <returns>
    ///     The path to the localization data.
    /// </returns>
    private static string GetEmbedFieldValuePath(string embedName)
    {
        return Path.Combine(GetEmbedPath(embedName), "field_value.json");
    }

    /// <summary>
    ///     Retrieves the localization data for a given embed's field title.
    /// </summary>
    /// <param name="embedName">
    ///     The name of the embed that the localization data is for.
    /// </param>
    /// <param name="languageCode">
    ///     The language code to retrieve the localization data for.
    /// </param>
    /// <returns>
    ///     The localization data for the embed's field title.
    /// </returns>
    public static string GetEmbedFieldTitle(string embedName, string languageCode)
    {
        string path = GetEmbedFieldTitlePath(embedName);
        
        return GetLocalization(path, languageCode);
    }
   
    /// <summary>
    ///     Retrieves the localization data for a given embed's field value.
    /// </summary>
    /// <param name="embedName">
    ///     The name of the embed that the localization data is for.
    /// </param>
    /// <param name="languageCode">
    ///     The language code to retrieve the localization data for.
    /// </param>
    /// <returns>
    ///     The localization data for the embed's field value.
    /// </returns>
    public static string GetEmbedFieldValue(string embedName, string languageCode)
    {
        string path = GetEmbedFieldValuePath(embedName);
        
        return GetLocalization(path, languageCode);
    }

    /// <summary>
    ///     Helper method to retrieve the localization data for a given path.
    /// </summary>
    /// <param name="path">
    ///     The path to the embed's localization data directory.
    /// </param>
    /// <param name="languageCode">
    ///     The language code to retrieve the localization data for.
    /// </param>
    /// <returns>
    ///     The localization data at the given path for the given language code.
    /// </returns>
    private static string GetLocalization(string path, string languageCode)
    {
        Dictionary<string, string> localization = RetrieveLocalizationAtPath(path);
        
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (localization.ContainsKey(languageCode))
        {
            return localization[languageCode];
        }
        
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (localization.ContainsKey("en-US"))
        {
            return localization["en-US"];
        }
        
        return "UNLOCALIZED";
    }

    /// <summary>
    ///     Gets the localization data for a given embed's custom localization field.
    /// </summary>
    /// <param name="embedName">
    ///     The name of the embed that the localization data is for.
    /// </param>
    /// <param name="fieldName">
    ///     The name of the field that the localization data is for.
    /// </param>
    /// <param name="languageCode">
    ///     The language code to retrieve the localization data for.
    /// </param>
    /// <returns>
    ///     The localization data for the embed's custom localization field.
    /// </returns>
    public static string GetEmbedCustomLocalization(string embedName, string fieldName, string languageCode)
    {
        string path = GetEmbedCustomLocalizationPath(embedName, fieldName, true);
        
        Dictionary<string, string> localization = RetrieveLocalizationAtPath(path);
        
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (localization.ContainsKey(languageCode))
        {
            return localization[languageCode];
        }
        
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (localization.ContainsKey("en-US"))
        {
            return localization["en-US"];
        }
        
        return "UNLOCALIZED";
    }

    /// <summary>
    ///     Helper method to retrieve the localization data's path for a given embed's custom field.
    /// </summary>
    /// <param name="embedName">
    ///     The name of the embed that the localization data is for.
    /// </param>
    /// <param name="fieldName">
    ///     The name of the field that the localization data is for.
    /// </param>
    /// <param name="createIfNotExists">
    ///     Whether or not to create the localization data directory if it does not exist.
    /// </param>
    /// <returns>
    ///     The path to the localization data.
    /// </returns>
    private static string GetEmbedCustomLocalizationPath(string embedName, string fieldName, bool createIfNotExists)
    {
        string path = GetEmbedPath(embedName);
        
        if (createIfNotExists)
        {
            Directory.CreateDirectory(path);
        }
        
        return Path.Combine(path, fieldName);
    }
}