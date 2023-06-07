using System;
using System.Collections.Generic;

namespace TranslatorBot;

public static class Commands
{
    public static List<string> GetCommandNames()
    {
        List<string> commandNames = new ();
        commandNames.Add("translate");
        commandNames.Add("translate from");
        commandNames.Add("reconnect to deepl");
        return commandNames;
    }

    public static List<string> GetCommandParameterNames(string commandName, bool noSpaces = false)
    {
        if (noSpaces)
        {
            return commandName switch
            {
                "translate" => new List<string> {"target-language", "text"},
                "translate from" => new List<string> {"source-language", "target-language", "text"},
                "reconnect to deepl" => new List<string>(),
                _ => throw new ArgumentOutOfRangeException(nameof(commandName), commandName, null)
            };
        }
        return commandName switch
        {
            "translate" => new List<string> {"target language", "text"},
            "translate from" => new List<string> {"source language", "target language", "text"},
            "reconnect to deepl" => new List<string>(),
            _ => throw new ArgumentOutOfRangeException(nameof(commandName), commandName, null)
        };
    }
}