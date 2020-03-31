using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using Qsor.Gameplay.osu.Screens;
using Qsor.Screens;

namespace Qsor
{
    [Cached]
    public class QsorGame : QsorBaseGame
    {
        public const uint CurrentTestmap = 756794 ; // TODO: Remove
        public const string CurrentTestmapName = "TheFatRat - Mayday (feat. Laura Brehm) (Voltaeyx) [[2B] Calling Out Mayday].osu"; // TODO: Remove
        
        private ScreenStack _stack;
        
        public Track ActiveTrack => BeatmapManager.ActiveBeatmap.Track;

        [BackgroundDependencyLoader]
        private void Load()
        {
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

            if (!DebugUtils.IsDebugBuild)
            {
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
            else
            {
                _stack.Push(new MainMenuScreen());
            }
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