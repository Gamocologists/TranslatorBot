using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TranslatorBot.Services;

/// <summary>
///     The class which handles logging.
///     This class is injected into the service provider.
///     This is a singleton service.
/// </summary>
public class LoggingService
{
    /// <summary>
    ///     The constructor for the logging service.
    /// </summary>
    /// <param name="discord">
    ///     The <see cref="DiscordSocketClient"/> which is used to log messages.
    ///     This is the bot's client.
    /// </param>
    public LoggingService(DiscordSocketClient discord)
    {
        LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

        discord.Log += OnLogAsync;
    }

    /// <summary>
    ///     The directory where the logs are stored.
    /// </summary>
    private string LogDirectory { get; }
    
    /// <summary>
    ///     The file where the logs are stored.
    /// </summary>
    private string LogFile => Path.Combine(LogDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.log");

    /// <summary>
    ///     The method which is called when a log message is received.
    /// </summary>
    /// <param name="msg">
    ///     The <see cref="LogMessage"/> which contains the log text to be logged.
    /// </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous operation of logging the message to the standard output.
    /// </returns>
    private Task OnLogAsync(LogMessage msg)
    {
        // Create log directory and file if needed
        if (!Directory.Exists(LogDirectory)) Directory.CreateDirectory(LogDirectory);
        if (!File.Exists(LogFile)) File.Create(LogFile).Dispose();

        // Write the log text to the current log file
        string logText =
            $"{DateTime.UtcNow:hh:mm:ss} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
        File.AppendAllText(LogFile, logText + "\n");

        // Write the log text to the console as well
        return Console.Out.WriteLineAsync(logText);
    }
}