using osu.Framework;

namespace Qsor
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var host = Host.GetSuitableHost("Qsor");
            using var game = new QsorGame();
            
            host.Run(game);
        }
    }
}