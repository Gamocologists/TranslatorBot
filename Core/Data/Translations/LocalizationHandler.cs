using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TranslatorBot.Data.Translations;

public static class LocalizationHandler
{
    public static readonly string BasePath = ComputeBasePath();

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
    
    public static string GetCommandLocalizationPath(string commandName, bool createIfNotFound = false)
    {
        string path = Path.Combine(BasePath, "COMMANDS", commandName);
        if (createIfNotFound && !Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        return path;
    }
    
    public static string GetCommandParameterLocalizationPath(string commandName, string parameter, bool createIfNotFound = false)
    {
        string path = Path.Combine(BasePath, "COMMANDS", commandName, "PARAMETERS", parameter);
        if (createIfNotFound && !Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        return path;
    }
    
    public static Dictionary<string, string> GetCommandNameLocalization(string commandName, bool createIfNotFound = false)
    {
        string path = GetCommandLocalizationPath(commandName, createIfNotFound);
        
        string namePath = Path.Combine(path, "names.json");
        
        return RetrieveLocalizationAtPath(namePath);
    }
    
    public static Dictionary<string, string> GetCommandDescriptionLocalization(string commandName, bool createIfNotFound = false)
    {
        string path = GetCommandLocalizationPath(commandName, createIfNotFound);
        
        string descriptionPath = Path.Combine(path, "descriptions.json");
        
        return RetrieveLocalizationAtPath(descriptionPath);
    }
    
    public static Dictionary<string, string> GetCommandParameterNameLocalization(string commandName, string parameter, bool createIfNotFound = false)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, createIfNotFound);
        
        string namePath = Path.Combine(path, "names.json");
        
        return RetrieveLocalizationAtPath(namePath);
    }
    
    public static Dictionary<string, string> GetCommandParameterDescriptionLocalization(string commandName, string parameter, bool createIfNotFound = false)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, createIfNotFound);
        
        string descriptionPath = Path.Combine(path, "descriptions.json");
        
        return RetrieveLocalizationAtPath(descriptionPath);
    }

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
    
    public static string AddOrSetCommandNameLocalization(string commandName, DiscordLanguage discordLanguage, string translation)
    {
        string path = GetCommandLocalizationPath(commandName, true);
        
        return AddOrSetLocalizationEntry(Path.Combine(path, "names.json"), discordLanguage, translation);
    }
    
    public static string AddOrSetCommandDescriptionLocalization(string commandName, DiscordLanguage discordLanguage, string translation)
    {
        string path = GetCommandLocalizationPath(commandName, true);
        
        return AddOrSetLocalizationEntry(Path.Combine(path, "descriptions.json"), discordLanguage, translation);
    }
    
    public static string AddOrSetCommandParameterNameLocalization(string commandName, string parameter, DiscordLanguage discordLanguage, string translation)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, true);
        
        return AddOrSetLocalizationEntry(Path.Combine(path, "names.json"), discordLanguage, translation);
    }
    
    public static string AddOrSetCommandParameterDescriptionLocalization(string commandName, string parameter, DiscordLanguage discordLanguage, string translation)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, true);
        
        return AddOrSetLocalizationEntry(Path.Combine(path, "descriptions.json"), discordLanguage, translation);
    }
    
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

    public static void GenerateBoilerplateLocalizationData(bool overwriteExisting = false)
    {
        List<string> commandNames = Commands.GetCommandNames();
        
        foreach (string commandName in commandNames)
        {
            GenerateBoilerplateCommandLocalizationData(commandName, overwriteExisting);
        }
        
    }

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

    private static void GenerateBoilerplateCommandNameLocalizationData(string path, bool overwriteExisting)
    {
        string namePath = Path.Combine(path, "names.json");
        
        GenerateBoilerplateLocalizationData(namePath, overwriteExisting);
    }
    
    private static void GenerateBoilerplateCommandDescriptionLocalizationData(string path, bool overwriteExisting)
    {
        string descriptionPath = Path.Combine(path, "descriptions.json");
        
        GenerateBoilerplateLocalizationData(descriptionPath, overwriteExisting);
    }
    
    private static void GenerateBoilerplateCommandParameterNameLocalizationData(string commandName, string parameter, bool overwriteExisting)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, true);
        
        string namePath = Path.Combine(path, "names.json");
        
        GenerateBoilerplateLocalizationData(namePath, overwriteExisting);
    }
    
    private static void GenerateBoilerplateCommandParameterDescriptionLocalizationData(string commandName, string parameter, bool overwriteExisting)
    {
        string path = GetCommandParameterLocalizationPath(commandName, parameter, true);
        
        string descriptionPath = Path.Combine(path, "descriptions.json");
        
        GenerateBoilerplateLocalizationData(descriptionPath, overwriteExisting);
    }

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

    public static List<Choice> GetCommandParameterChoicesLocalization(string originatingCommand, string parameter,
        Dictionary<string, (string name, int value)> choices)
    {
        List<Choice> results = new();
        
        foreach (KeyValuePair<string, (string name, int value)> choice in choices)
        {
            string path = GetCommandParameterChoiceLocalizationPath(originatingCommand, parameter, choice.Key, true);
            
            Dictionary<string, string> localization = RetrieveLocalizationAtPath(path);
            
            string name = choice.Value.name;
            int value = choice.Value.value;
            
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

    private static string GetCommandParameterChoiceLocalizationPath(string originatingCommand, string parameter, string choice, bool createIfNotExists)
    {
        string path = GetCommandParameterLocalizationPath(originatingCommand, parameter, createIfNotExists);
        
        return Path.Combine(path, "OPTIONS", choice);
    }
}