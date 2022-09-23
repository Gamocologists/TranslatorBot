using System.Threading.Tasks;

namespace TranslatorBot;

internal static class Program
{
    public static Task Main(string[] args)
    {
        return Startup.RunAsync(args);
    }
}