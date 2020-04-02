using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using Qsor.Beatmaps;
using Qsor.Database;
using Qsor.Online;

namespace Qsor
{
    public class QsorBaseGame : Game
    {
        protected BeatmapManager BeatmapManager;
        protected UserManager UserManager;
        
        protected BeatmapMirrorAccess BeatmapMirrorAccess;
        
        protected QsorDbContextFactory QsorDbContextFactory;
        
        private DependencyContainer _dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            _dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void Load(Storage storage)
        {
            _dependencies.CacheAs(BeatmapManager = new BeatmapManager());
            _dependencies.Cache(UserManager = new UserManager());
            
            _dependencies.Cache(BeatmapMirrorAccess = new BeatmapMirrorAccess());
            
            _dependencies.Cache(QsorDbContextFactory = new QsorDbContextFactory(storage));
            
            _dependencies.CacheAs(this);
            
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
            

            QsorDbContextFactory.Get().Migrate();
        }
    }
}