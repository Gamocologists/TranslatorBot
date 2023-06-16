using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace TranslatorBot.Modules.Translation;

public static class SlashCommandDispatcher
{
    public static async Task ExecuteSlashCommand(SocketSlashCommand slashCommand)
    {
        SocketSlashCommandData data = slashCommand.Data;
        switch (data.Name)
        {
            case "translate":
                await CheckTranslateCommand(data);

                (bool isSuccess, string targetLanguage, string text) translateResult = 
                    await CheckTranslateCommand(data);
                
                if (translateResult.isSuccess)
                {
                    Embed embed = await TranslationModule.Translate(translateResult.targetLanguage, slashCommand.UserLocale, translateResult.text);
                    await slashCommand.RespondAsync(embed: embed);
                }
                break;
            case "translate-from":
                
                (bool isSuccess, string sourceLanguage, string targetLanguage, string text) translateFromCommand = 
                    await CheckTranslateFromCommand(data);
                
                if (translateFromCommand.isSuccess)
                {
                    Embed embed = await TranslationModule.TranslateFrom(translateFromCommand.sourceLanguage,
                        translateFromCommand.targetLanguage,
                        slashCommand.UserLocale, translateFromCommand.text);
                    await slashCommand.RespondAsync(embed: embed);
                }
                break;
            case "reconnect-to-deepl":
            {
                if (data.Options.Count == 0)
                {
                    Embed embed = TranslationModule.ReconnectToTranslationApi(slashCommand.UserLocale);
                    await slashCommand.RespondAsync(embed: embed);
                }

                break;
            }
            default:
                await Console.Out.WriteLineAsync($"Unknown slash command: {slashCommand.Data.Name}");
                break;
        }
    }

    private static async Task<(bool wasSuccess, string targetLanguage, string text)> CheckTranslateCommand(SocketSlashCommandData data)
    {
        int count = data.Options.Count;
        if (count != 2)
        {
            await Console.Out.WriteLineAsync($"Expected 2 options, got {count}");
            return (false, "", "");
        }

        int index = 0;

        SocketSlashCommandDataOption targetLanguageOption = null;
        SocketSlashCommandDataOption textOption = null;
        foreach (SocketSlashCommandDataOption socketSlashCommandDataOption in data.Options)
        {
            if (index == 0)
            {
                targetLanguageOption = socketSlashCommandDataOption;
            }
            else if (index == 1)
            {
                textOption = socketSlashCommandDataOption;
            }
            else
            {
                await Console.Out.WriteLineAsync($"Unexpected index {index}");
                break;
            }

            index += 1;
        }
        
        return await AssertOptions(targetLanguageOption!, textOption!);
    }
    
    private static async Task<(bool wasSuccess, string sourceLanguage, 
        string targetLanguage, string text)> CheckTranslateFromCommand(
        SocketSlashCommandData data)
    {
        int count = data.Options.Count;
        if (count != 3)
        {
            await Console.Out.WriteLineAsync($"Expected 2 options, got {count}");
            return (false, "", "", "");
        }

        int index = 0;

        SocketSlashCommandDataOption sourceLanguageOption = null;
        SocketSlashCommandDataOption targetLanguageOption = null;
        SocketSlashCommandDataOption textOption = null;
        foreach (SocketSlashCommandDataOption socketSlashCommandDataOption in data.Options)
        {
            if (index == 0)
            {
                sourceLanguageOption = socketSlashCommandDataOption;
            }
            else if (index == 1)
            {
                targetLanguageOption = socketSlashCommandDataOption;
            } 
            else if (index == 2)
            {
                textOption = socketSlashCommandDataOption;
            }
            else
            {
                await Console.Out.WriteLineAsync($"Unexpected index {index}");
                break;
            }

            index += 1;
        }
        
        return await AssertOptions(sourceLanguageOption!, 
            targetLanguageOption!, textOption!);
    }

    private static async Task<(bool wasSuccess, string targetLanguage, string text)> AssertOptions(
        SocketSlashCommandDataOption targetLanguageOption, 
        SocketSlashCommandDataOption textOption)
    {
        if (targetLanguageOption!.Name != "target-language")
        {
            await Console.Out.WriteLineAsync(
                $"Expected name target-language, got {targetLanguageOption.Name}");
            return (false, "", "");
        }

        if (targetLanguageOption.Type != ApplicationCommandOptionType.String)
        {
            await Console.Out.WriteLineAsync(
                $"Expected type {ApplicationCommandOptionType.String}, got {targetLanguageOption.Type}");
            return (false, "", "");
        }
        
        if (targetLanguageOption.Value == null)
        {
            await Console.Out.WriteLineAsync($"Expected value, got null");
            return (false, "", "");
        }
        
        if (textOption!.Name != "text")
        {
            await Console.Out.WriteLineAsync(
                $"Expected name text, got {textOption.Name}");
            return (false, "", "");
        }

        if (textOption.Type != ApplicationCommandOptionType.String)
        {
            await Console.Out.WriteLineAsync(
                $"Expected type {ApplicationCommandOptionType.String}, got {textOption.Type}");
            return (false, "", "");
        }

        if (textOption.Value == null)
        {
            await Console.Out.WriteLineAsync($"Expected value, got null");
            return (false, "", "");
        }
        
        return (true, targetLanguageOption.Value.ToString()!, textOption.Value.ToString()!);
    }
    
    private static async Task<(bool wasSuccess, string sourceLanguage, string targetLanguage, string text)> AssertOptions(
        SocketSlashCommandDataOption sourceLanguageOption,
        SocketSlashCommandDataOption targetLanguageOption, 
        SocketSlashCommandDataOption textOption)
    {
        if (sourceLanguageOption!.Name != "source-language")
        {
            await Console.Out.WriteLineAsync(
                $"Expected name source-language, got {sourceLanguageOption.Name}");
            return (false, "", "", "");
        }
        
        if (sourceLanguageOption.Type != ApplicationCommandOptionType.String)
        {
            await Console.Out.WriteLineAsync(
                $"Expected type {ApplicationCommandOptionType.String}, got {sourceLanguageOption.Type}");
            return (false, "", "", "");
        }
        
        if (sourceLanguageOption.Value == null)
        {
            await Console.Out.WriteLineAsync($"Expected value, got null");
            return (false, "", "", "");
        }
        
        if (targetLanguageOption!.Name != "target-language")
        {
            await Console.Out.WriteLineAsync(
                $"Expected name target-language, got {targetLanguageOption.Name}");
            return (false, "", "", "");
        }

        if (targetLanguageOption.Type != ApplicationCommandOptionType.String)
        {
            await Console.Out.WriteLineAsync(
                $"Expected type {ApplicationCommandOptionType.String}, got {targetLanguageOption.Type}");
            return (false, "", "", "");
        }
        
        if (targetLanguageOption.Value == null)
        {
            await Console.Out.WriteLineAsync($"Expected value, got null");
            return (false, "", "", "");
        }
        
        if (textOption!.Name != "text")
        {
            await Console.Out.WriteLineAsync(
                $"Expected name text, got {textOption.Name}");
            return (false, "", "", "");
        }

        if (textOption.Type != ApplicationCommandOptionType.String)
        {
            await Console.Out.WriteLineAsync(
                $"Expected type {ApplicationCommandOptionType.String}, got {textOption.Type}");
            return (false, "", "", "");
        }

        if (textOption.Value == null)
        {
            await Console.Out.WriteLineAsync($"Expected value, got null");
            return (false, "", "", "");
        }
        
        return (true, sourceLanguageOption.Value.ToString()!, 
            targetLanguageOption.Value.ToString()!, 
            textOption.Value.ToString()!);
    }
}