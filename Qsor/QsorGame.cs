
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using osuTK;
using Qsor.Gameplay.osu;
using Qsor.Gameplay.osu.Screens;
using Qsor.Online;

namespace Qsor
{
    public class QsorGame : Game
    {
        public const uint CURRENT_TESTMAP = 90935; // TODO: Remove
        public const string CURRENT_TESTMAP_NAME = "IOSYS - Endless Tewi-ma Park (Lanturn) [Tewi 2B Expert Edition].osu"; // TODO: Remove
        
        private ScreenStack _stack;

        private BeatmapManager BeatmapManager;
        private BeatmapMirrorAccess BeatmapMirrorAccess;
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public Track ActiveTrack => BeatmapManager.Song;

        [BackgroundDependencyLoader]
        private void Load()
        {
            dependencies.Cache(BeatmapMirrorAccess = new BeatmapMirrorAccess());
            dependencies.CacheAs(BeatmapManager = new BeatmapManager());
            dependencies.CacheAs(this);
            
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
            
            Audio.Frequency.Set(1);
            Audio.Volume.Set(.2);
            
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