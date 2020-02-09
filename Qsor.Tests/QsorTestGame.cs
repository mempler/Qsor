using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace Qsor.Tests
{
    public class QsorTestGame : Game
    {
        [BackgroundDependencyLoader]
        private void Load()
        {
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorTestGame).Assembly), @"Resources"));
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
            
            Add(new TestBrowser("Qsor.Tests"));
        }
    }
}