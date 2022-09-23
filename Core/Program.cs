using System.Threading.Tasks;
using TranslatorBot;

namespace GamocologistBot
{
    internal static class Program
    {
        public static Task Main(string[] args)
        {
            return Startup.RunAsync(args);
        }
    }
}