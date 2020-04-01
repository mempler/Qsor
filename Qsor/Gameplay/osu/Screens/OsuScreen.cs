using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Qsor.Beatmaps;

namespace Qsor.Gameplay.osu.Screens
{
    public class OsuScreen : Screen
    {
        [Resolved]
        private AudioManager AudioManager { get; set; }

        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }
        
        [Resolved]
        private Storage Storage { get; set; }

        private BeatmapContainer _beatmapContainer;

        [BackgroundDependencyLoader]
        private void Load()
        {
            var beatmapStorage = Storage.GetStorageForDirectory($"./Songs/{QsorGame.CurrentTestmap}");
            _beatmapContainer = BeatmapManager.LoadBeatmap(beatmapStorage, QsorGame.CurrentTestmapName); // TODO: Remove
            
            AddInternal(_beatmapContainer);
        }
        
        protected override void LoadComplete()
        {
            _beatmapContainer.PlayBeatmap();
            
            base.LoadComplete();
        }
        
        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(2000, Easing.InQuad);
        }
    }
}