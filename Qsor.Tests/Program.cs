using osu.Framework;

namespace Qsor.Tests
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var host = Host.GetSuitableHost(@"Qsor", true);
            host.Run(new QsorTestGame());
        }
    }
}