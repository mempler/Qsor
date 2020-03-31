using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using Qsor.Gameplay.osu;
using Qsor.Online;

namespace Qsor
{
    public class QsorBaseGame : Game
    {
        protected BeatmapManager BeatmapManager;
        protected BeatmapMirrorAccess BeatmapMirrorAccess;
        protected UserManager UserManager;
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void Load()
        {
            dependencies.Cache(BeatmapMirrorAccess = new BeatmapMirrorAccess());
            dependencies.CacheAs(BeatmapManager = new BeatmapManager());
            dependencies.CacheAs(this);

            dependencies.Cache(UserManager = new UserManager());
            
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
        }
    }
}