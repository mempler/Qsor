
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using osuTK.Input;
using Qsor.Gameplay.osu;
using Qsor.Gameplay.osu.Screens;
using Qsor.Online;
using Qsor.Screens;

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
            
            _stack = new ScreenStack
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            };
            Add(_stack);
            
            _stack.Push(new IntroScreen());

            BeatmapManager.OnLoadComplete += (d) =>
            {
                _stack.Exit();

                Scheduler.AddDelayed(() => _stack.Push(new BeatmapScreen
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    FillMode = FillMode.Fill,
                }), 2000);
            };
            
            Scheduler.AddDelayed(() => AddInternal(BeatmapManager), 6000);
        }
        
        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    Audio.Frequency.Value -= .1;
                    return true;
                case Key.Up:
                    Audio.Frequency.Value += .1;
                    return true;
                case Key.Space:
                    if (!ActiveTrack.IsRunning)
                        ActiveTrack.Start();
                    else
                        ActiveTrack.Stop();
                    return true;
                default:
                    return false;
            }
        }
    }
}