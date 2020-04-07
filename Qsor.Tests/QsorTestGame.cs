using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Testing;
using Qsor.Game;

namespace Qsor.Tests
{
    public class QsorTestGame : QsorBaseGame
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorTestGame).Assembly), @"Resources"));

            Add(new TestBrowser("Qsor.Tests"));
        }

        public QsorTestGame(string[] args) : base(args)
        {
        }
    }
}