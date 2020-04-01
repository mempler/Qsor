using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;
using Qsor.Gameplay.osu.Screens;
using Qsor.Screens;
using Qsor.Screens.Menu;

namespace Qsor
{
    [Cached]
    public class QsorGame : QsorBaseGame
    {
        public const uint CurrentTestmap = 605745; // TODO: Remove
        public const string CurrentTestmapName = "Pegboard Nerds - We Are One (Original Vocal Mix) (Frey) [Night Begins].osu"; // TODO: Remove
        
        private ScreenStack _stack;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Audio.Frequency.Set(1);
            Audio.Volume.Set(.05);
            
            _stack = new ScreenStack(false);
            Add(_stack);

            if (!DebugUtils.IsDebugBuild)
            {
                _stack.Anchor = Anchor.Centre;
                _stack.Origin = Anchor.Centre;
                
                _stack.Push(new IntroScreen());
   
                AddInternal(BeatmapManager);
            
                Scheduler.AddDelayed(() =>
                {
                    _stack.Exit();
                    
                    Scheduler.AddDelayed(() => _stack.Push(new MainMenuScreen()), 2000);
                }, 6000);
            }
            else
            {
                _stack.Push(new MainMenuScreen());
            }
        }

        public void PushScreen(Screen screen)
        {
            _stack.Push(screen);
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
                    if (!BeatmapManager.WorkingBeatmap.Value.Track.IsRunning)
                        BeatmapManager.WorkingBeatmap.Value.Track.Start();
                    else
                        BeatmapManager.WorkingBeatmap.Value.Track.Stop();
                    return true;
                default:
                    return false;
            }
        }
    }
}