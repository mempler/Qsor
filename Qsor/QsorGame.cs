
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using osuTK;
using Qsor.osu;
using Qsor.Screens;

namespace Qsor
{
    public class QsorGame : Game
    {
        private ScreenStack _stack;

        private BeatmapManager BeatmapManager;
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public Track ActiveTrack => BeatmapManager.Song;

        [BackgroundDependencyLoader]
        private void Load()
        {
            AddInternal(BeatmapManager = new BeatmapManager());
            dependencies.CacheAs(BeatmapManager);
            dependencies.CacheAs(this);
            
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(QsorGame).Assembly), @"Resources"));
            
            Audio.Frequency.Set(1);
            Audio.Volume.Set(.2);
            
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