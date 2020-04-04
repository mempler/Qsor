using System.Threading.Tasks;
using osu.Framework;
using Qsor.Game;

namespace Qsor.Desktop
{
    internal static class Program
    {
        public static async Task Main()
        {
            using var host = Host.GetSuitableHost("Qsor");
            using var game = new QsorGame();
            
            host.Run(game);
        }
    }
}