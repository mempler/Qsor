using System;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK.Input;
using Qsor.Game.Input;
using Qsor.Game.Screens;

namespace Qsor.Game
{
    [Cached]
    public class QsorGame : QsorBaseGame, IKeyBindingHandler<GlobalAction>
    {
        private GlobalKeyBindingInputHandler KeyBindingInputHandler;
        private ScreenStack _stack;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Audio.Frequency.Set(1);
            Audio.Volume.Set(.05);

            _stack = new ScreenStack(false);
            Add(_stack);
            
            Window.Title = $"Qsor - {Version}";
            
            AddInternal(KeyBindingInputHandler = new GlobalKeyBindingInputHandler(this));
            
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

        public bool OnPressed(GlobalAction action)
        {
            switch (action)
            {
                case GlobalAction.ToggleOptions:
                    if (SettingsOverlay.IsShown)
                        SettingsOverlay.Hide();
                    else
                        SettingsOverlay.Show();
                    break;

                case GlobalAction.ExitOverlay:
                    if (SettingsOverlay.IsShown)
                        SettingsOverlay.Hide();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
            
            return true;
        }

        public void OnReleased(GlobalAction action)
        {
        }
    }
}