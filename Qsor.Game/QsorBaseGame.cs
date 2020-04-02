using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using Qsor.Game.Beatmaps;
using Qsor.Game.Database;
using Qsor.Game.Online;
using Qsor.Game.Overlays;

namespace Qsor.Game
{
    public class QsorBaseGame : osu.Framework.Game
    {
        protected BeatmapManager BeatmapManager;
        protected UserManager UserManager;
        
        protected BeatmapMirrorAccess BeatmapMirrorAccess;
        
        protected QsorDbContextFactory QsorDbContextFactory;

        protected NotificationOverlay NotificationOverlay;
        
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
            _dependencies.Cache(NotificationOverlay = new NotificationOverlay());
            
            _dependencies.CacheAs(this);
            
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
            
            QsorDbContextFactory.Get().Migrate();
            
            AddInternal(BeatmapManager);
            AddInternal(NotificationOverlay);
        }
    }
}