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
        public const uint CurrentTestmap = 690556; // TODO: Remove
        public const string CurrentTestmapName = "Virtual Riot - Stay For A While (ProfessionalBox) [Don't make me Lonely].osu"; // TODO: Remove
        
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
    }
}