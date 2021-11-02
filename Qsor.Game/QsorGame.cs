using System;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osuTK.Input;
using Qsor.Game.Graphics.UserInterface.Screens;
using Qsor.Game.Graphics.UserInterface.Screens.MainMenu;
using Qsor.Game.Input;

namespace Qsor.Game
{
    [Cached]
    public class QsorGame : QsorBaseGame, IKeyBindingHandler<GlobalAction>
    {
        private GlobalKeyBindingInputHandler _keyBindingInputHandler;

        [BackgroundDependencyLoader]
        private void Load()
        {
            Audio.Frequency.Set(1);
            Audio.Volume.Set(.35);

            Window.Title = $"Qsor - {Version}";
            
            AddInternal(_keyBindingInputHandler = new GlobalKeyBindingInputHandler(this));
            
            if (!DebugUtils.IsDebugBuild)
            {
                PushScreen(new IntroScreen());
                
                Scheduler.AddDelayed(() =>
                {
                    ExitScreen();
                    
                    Scheduler.AddDelayed(() => PushScreen(new MainMenuScreen()), 2000);
                }, 6000);
            }
            else
            {
                PushScreen(new MainMenuScreen());
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

        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            switch (e.Action)
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
                    throw new ArgumentOutOfRangeException(nameof(e.Action), e.Action, null);
            }
            
            return true;
        }

        public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
        {
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (SettingsOverlay.IsShown && !SettingsOverlay.IsHovered)
                SettingsOverlay.Hide();
            
            return base.OnClick(e);
        }
    }
}