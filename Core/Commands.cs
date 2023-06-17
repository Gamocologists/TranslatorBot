using System;
using System.Collections.Generic;

namespace TranslatorBot;

/// <summary>
///     The class which contains all the commands and their parameters.
/// </summary>
public static class Commands
{
    /// <summary>
    ///     The method which returns all the command names.
    /// </summary>
    /// <returns>
    ///     A <see cref="List{T}"/> of <see cref="string"/>s containing all the command names.
    /// </returns>
    public static List<string> GetCommandNames()
    {
        List<string> commandNames = new ();
        commandNames.Add("translate");
        commandNames.Add("translate from");
        commandNames.Add("reconnect to deepl");
        return commandNames;
    }

    /// <summary>
    ///     The method which returns all the command parameter names.
    /// </summary>
    /// <param name="commandName">
    ///     The name of the command to get the parameter names for.
    /// </param>
    /// <param name="noSpaces">
    ///     Whether or not to return the parameter names without spaces.
    /// </param>
    /// <returns>
    ///     A <see cref="List{T}"/> of <see cref="string"/>s containing all the command parameter names.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the command name is not recognized.
    /// </exception>
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