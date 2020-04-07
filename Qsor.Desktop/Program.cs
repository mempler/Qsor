using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Development;
using Qsor.Desktop.Updater;
using Qsor.Game;

namespace Qsor.Desktop
{
    internal static class Program
    {
        public static async Task Main(params string[] args)
        {
            using var host = Host.GetSuitableHost("Qsor");
            using var game = new QsorGame(args)
            {
                Updater = DebugUtils.IsDebugBuild
                    ? (Game.Updater.Updater) new DummyUpdater()
                    : (Game.Updater.Updater) new SquirrelUpdater()
            };
            
            host.Run(game);
        }
    }
}