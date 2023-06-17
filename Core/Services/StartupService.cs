using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using TranslatorBot.Modules.Translation;

namespace TranslatorBot.Services;

/// <summary>
///     Service run at startup to initialize all key services and fetch configurations.
///     Does not handle logging.
/// </summary>
public class StartupService
{
    /// <summary>
    ///     The <see cref="IConfigurationRoot" /> _config attribute stores the
    ///     bot's configuration.
    /// </summary>
    private readonly IConfigurationRoot _config;

    /// <summary>
    ///     Represents the discord account associated with the bot.
    /// </summary>
    private readonly DiscordSocketClient _discord;

    /// <summary>
    ///     The constructor for the startup service.
    /// </summary>
    /// <param name="discord">
    ///     The <see cref="DiscordSocketClient" /> _discord attribute represents the
    ///     discord account associated with the bot.
    /// </param>
    /// <param name="config">
    ///     The <see cref="IConfigurationRoot" /> _config attribute stores the
    ///     bot's configuration.
    /// </param>
    public StartupService(DiscordSocketClient discord,
        IConfigurationRoot config)
    {
        _config = config;
        _discord = discord;
    }

    /// <summary>
    ///     Starts services, logs in and loads module.
    /// </summary>
    /// <exception cref="Exception">
    ///     If the bot token is empty, an exception is thrown.
    /// </exception>
    public async Task StartAsync()
    {
        string discordToken = _config["tokens:discord"]; // Get the discord token from the config file
        if (string.IsNullOrWhiteSpace(discordToken))
            throw new FileLoadException(
                "Please enter your bots token into the `appsettings.json` file found in the Core directory.");

        await _discord.LoginAsync(TokenType.Bot, discordToken); // Login to discord
        await _discord.StartAsync(); // Connect to the websocket

        _discord.Ready += async () =>
        {
            IReadOnlyCollection<SocketApplicationCommand>? existingCommands = await _discord.GetGlobalApplicationCommandsAsync();
            foreach (SocketApplicationCommand? command in existingCommands)
            {
                await command.DeleteAsync();
            }
            await _discord.SetStatusAsync(UserStatus.Online);
            Game game = GenerateRichPresence();
            await _discord.SetActivityAsync(game);
            await AddSlashCommands();
            await Task.CompletedTask;

            _discord.SlashCommandExecuted += SlashCommandDispatcher.ExecuteSlashCommand;
        };
    }

    /// <summary>
    ///     Adds slash commands to the bot.
    /// </summary>
    private async Task AddSlashCommands()
    {
        SlashCommandProperties translateCommandProperties = SlashCommandGenerator.GenerateTranslationSlashCommand();
        SlashCommandProperties translateFromCommandProperties =
            SlashCommandGenerator.GenerateTranslationFromSlashCommand();
        SlashCommandProperties reconnectToDeepLCommandProperties =
            SlashCommandGenerator.GenerateReconnectToDeepLSlashCommand();
        
        await _discord.CreateGlobalApplicationCommandAsync(translateCommandProperties);
        await _discord.CreateGlobalApplicationCommandAsync(translateFromCommandProperties);
        await _discord.CreateGlobalApplicationCommandAsync(reconnectToDeepLCommandProperties);
    }

    /// <summary>
    ///     Generates the rich presence for the bot.
    /// </summary>
    /// <returns>
    ///     The <see cref="Game" /> object representing the rich presence.
    ///     The rich presence is currently set to "Translating".
    /// </returns>
    private Game GenerateRichPresence()
    {
        Game game = new ("Translating");
        return game;
    }
}