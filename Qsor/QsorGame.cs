
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using Qsor.Gameplay.osu;
using Qsor.Gameplay.osu.Screens;
using Qsor.Online;

namespace Qsor
{
    [Cached]
    public class QsorGame : Game
    {
        public const uint CurrentTestmap = 756794 ; // TODO: Remove
        public const string CurrentTestmapName = "TheFatRat - Mayday (feat. Laura Brehm) (Voltaeyx) [[2B] Calling Out Mayday].osu"; // TODO: Remove
        
        private ScreenStack _stack;

        private BeatmapManager BeatmapManager;
        private BeatmapMirrorAccess BeatmapMirrorAccess;
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public Track ActiveTrack => BeatmapManager.ActiveBeatmap.Track;

        [BackgroundDependencyLoader]
        private void Load()
        {
            dependencies.Cache(BeatmapMirrorAccess = new BeatmapMirrorAccess());
            dependencies.CacheAs(BeatmapManager = new BeatmapManager());
            dependencies.CacheAs(this);
            
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
            
            Audio.Frequency.Set(1);
            Audio.Volume.Set(.05);
            
            AddInternal(BeatmapManager);
            
            _stack = new ScreenStack
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            };
            Add(_stack);
            
            _stack.Push(new BeatmapScreen
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
        }
    }
}