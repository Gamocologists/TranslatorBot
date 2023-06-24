using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TranslatorBot.Services;

namespace TranslatorBot;

/// <summary>
///     The class which starts and runs the bot.
/// </summary>
public class Startup
{
    /// <summary>
    ///     The constructor for the startup class.
    ///     This is where the configuration is loaded.
    /// </summary>
    public Startup()
    {
        string baseDirectory = AppContext.BaseDirectory;
        string configFilePath = $"{baseDirectory}/../../../";
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(configFilePath)
            .AddJsonFile("appsettings.json");
        Configuration = builder.Build();
    }

    /// <summary>
    ///     The configuration for the bot. Includes the token and optionally the prefix or other settings.
    /// </summary>
    public IConfigurationRoot Configuration { get; }

    /// <summary>
    ///     The method which starts the bot.
    /// </summary>
    public static async Task RunBotAsync()
    {
        Startup startup = new();
        await startup.RunAsync();
    }

    /// <summary>
    ///     The method which runs the bot.
    /// </summary>
    public async Task RunAsync()
    {
        ServiceCollection services = new(); // Create a new instance of a service collection
        ConfigureServices(services);

        ServiceProvider provider = services.BuildServiceProvider(); // Build the service provider
        provider.GetRequiredService<LoggingService>(); // Start the logging service

        // Start the startup service and keep the program alive
        await provider.GetRequiredService<StartupService>().StartAsync();
        await Task.Delay(-1);
    }

    /// <summary>
    ///     The method which configures the services for the bot.
    /// </summary>
    /// <param name="services">
    ///     The service collection which will be used to configure the services.
    /// </param>
    private void ConfigureServices(IServiceCollection services)
    {
        DiscordSocketClient bot = new(new DiscordSocketConfig
        {
            // Add discord to the collection
            LogLevel = LogSeverity.Verbose, // Tell the logger to give Verbose amount of info
            MessageCacheSize = 1000,
            GatewayIntents = GatewayIntents.Guilds
        });

        //Bot = new Bot(bot);

        services.AddSingleton(bot)
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                // Add the command service to the collection
                LogLevel = LogSeverity.Verbose, // Tell the logger to give Verbose amount of info
                DefaultRunMode = RunMode.Async // Force all commands to run async by default
            }))
            .AddSingleton<StartupService>() // Add startup service to the collection
            .AddSingleton<LoggingService>() // Add logging service to the collection
            .AddSingleton<Random>() // Add random to the collection
            .AddSingleton(Configuration); // Add the configuration to the collection
    }
}