using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using Qsor.Game.Beatmaps;
using Qsor.Game.Configuration;

namespace Qsor.Game.Gameplay.osu.Screens
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
        private void Load(QsorConfigManager configManager)
        {
            var beatmapStorage = Storage.GetStorageForDirectory($"./Songs/{configManager.Get<int>(QsorSetting.BeatmapSetId)}");
            _beatmapContainer = BeatmapManager.LoadBeatmap(beatmapStorage, configManager.Get<string>(QsorSetting.BeatmapFile)); // TODO: Remove
            
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