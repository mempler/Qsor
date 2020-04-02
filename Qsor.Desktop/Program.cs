using System.Threading.Tasks;
using osu.Framework;
using Qsor.Game;

namespace Qsor.Desktop
{
    internal class Program
    {
        public static async Task Main()
        {
            using var host = Host.GetSuitableHost("Qsor.Game");
            using var game = new QsorGame();
            
            host.Run(game);
        }
    }
}