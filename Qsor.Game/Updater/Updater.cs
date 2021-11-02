using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using Qsor.Game.Graphics.UserInterface.Overlays;

namespace Qsor.Game.Updater
{
    [Cached]
    public abstract class Updater : Component
    {
        public readonly Bindable<UpdaterStatus> BindableStatus = new();
        public readonly BindableFloat BindableProgress = new();
        
        public abstract void CheckAvailable();
        public abstract void UpdateGame();

        [Resolved]
        private UpdaterOverlay UpdaterOverlay { get; set; }

        [BackgroundDependencyLoader]
        private void Load()
        {
            BindableStatus.ValueChanged += e =>
            {
                switch (e.NewValue)
                {
                    case UpdaterStatus.Latest:
                        break; // Ignore
                    
                    case UpdaterStatus.Pending:
                    case UpdaterStatus.Downloading:
                        UpdaterOverlay.Show();
                        break;
                    
                    case UpdaterStatus.Ready:
                        UpdaterOverlay.Text = new LocalisableString("Click here to restart!");
                        UpdaterOverlay.Show();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };

            BindableProgress.ValueChanged += e =>
                UpdaterOverlay.Text = new LocalisableString($"Updating Qsor... {e.NewValue}%");
        }
    }
}