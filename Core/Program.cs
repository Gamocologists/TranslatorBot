using System.Threading.Tasks;

namespace TranslatorBot;

internal static class Program
{
    public static Task Main()
    {
        return Startup.RunBotAsync();
    }
}