using Discord;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;
using Qsor.Game.Screens;

namespace Qsor.Game
{
    [Cached]
    public class QsorGame : QsorBaseGame
    {
        private ScreenStack _stack;
        
        public Discord.Discord DiscordGameSdk;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Audio.Frequency.Set(1);
            Audio.Volume.Set(.05);
            
            _stack = new ScreenStack(false);
            Add(_stack);
            
            Window.Title = $"Qsor - {Version}";
            
            // Discord Game SDK is not thread safe, it must run on the Update Thread
            Scheduler.Add(() =>
            {
                DiscordGameSdk = new Discord.Discord(694816216442863667, 0);
                
                DiscordGameSdk
                    .GetActivityManager()
                    .UpdateActivity(new Activity
                {
                    Name = "Qsor",
                    Details = $"Running Qsor {Version}",
                    Assets = new ActivityAssets
                    {
                        LargeImage = "logo",
                        LargeText = "Qsor"
                    },
                    State = "cup o’ cheater tears"
                }, e => {});
            });

            if (!DebugUtils.IsDebugBuild)
            {
                _stack.Anchor = Anchor.Centre;
                _stack.Origin = Anchor.Centre;
                
                _stack.Push(new IntroScreen());
                
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

        public void ExitScreen()
        {
            _stack.Exit();
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

        public QsorGame(string[] args) : base(args)
        {
        }

        protected override void UpdateAfterChildren()
        {
            DiscordGameSdk?.RunCallbacks();
        }
    }
}