using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace Qsor.Gameplay.osu.Screens
{
    public class BeatmapScreen : Screen
    {
        [Resolved]
        private AudioManager AudioManager { get; set; }

        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }
        
        protected override void LoadComplete()
        {
            BeatmapManager.LoadBeatmap($"./Songs/{QsorGame.CurrentTestmap}/{QsorGame.CurrentTestmapName}"); // TODO: Remove
            BeatmapManager.PlayBeatmap();
            
            base.LoadComplete();
        }
        
        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(2000, Easing.OutQuint);
        }
    }
}