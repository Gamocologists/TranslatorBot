using System.Threading.Tasks;

namespace TranslatorBot;

/// <summary>
///    The class which starts and runs the bot.
/// </summary>
internal static class Program
{
    /// <summary>
    ///     The method which starts the bot.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous operation of running the bot.
    /// </returns>
    public static Task Main()
    {
        return Startup.RunBotAsync();
    }
}