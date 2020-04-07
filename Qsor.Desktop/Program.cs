using System.Threading.Tasks;
using osu.Framework;
using Qsor.Desktop.Updater;
using Qsor.Game;

namespace Qsor.Desktop
{
    internal static class Program
    {
        public static async Task Main(params string[] args)
        {
            using var host = Host.GetSuitableHost("Qsor");
            using var updater = new SquirrelUpdater();
            using var game = new QsorGame(args) { Updater = updater };
            
            host.Run(game);
        }
    }
}