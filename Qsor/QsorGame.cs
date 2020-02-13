
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
    [Cached]
    public class QsorGame : Game
    {
        public const uint CurrentTestmap = 756794 ; // TODO: Remove
        public const string CurrentTestmapName = "TheFatRat - Mayday (feat. Laura Brehm) (Voltaeyx) [[2B] Calling Out Mayday].osu"; // TODO: Remove
        
        private ScreenStack _stack;

        [Cached]
        public readonly BeatmapManager BeatmapManager = new BeatmapManager();
        
        [Cached]
        public readonly BeatmapMirrorAccess BeatmapMirrorAccess = new BeatmapMirrorAccess();

        public Track ActiveTrack => BeatmapManager.Song;

        [BackgroundDependencyLoader]
        private void Load()
        {
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