using osu.Framework;

namespace Qsor.Desktop
{
    internal static class Program
    {
        public static void Main(params string[] args)
        {
            using var host = Host.GetSuitableDesktopHost("Qsor");
            using var game = new QsorGameDesktop(args);
            
            host.Run(game);
        }
    }
}