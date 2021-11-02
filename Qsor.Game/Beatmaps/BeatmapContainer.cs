using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using Qsor.Game.Graphics.Containers;

namespace Qsor.Game.Beatmaps
{
    public class BeatmapContainer : Container
    {
        private BackgroundImageContainer _background;
        public Bindable<WorkingBeatmap> WorkingBeatmap { get; } = new();
        
        [Resolved]
        private AudioManager Audio { get; set; }

        public BeatmapContainer(Bindable<WorkingBeatmap> beatmap)
        {
            WorkingBeatmap.BindTo(beatmap);
        }
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            FillMode = FillMode.Fill;
            
            LoadComponent(WorkingBeatmap.Value);

            AddInternal(_background = new BackgroundImageContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
            
            _background.SetTexture(WorkingBeatmap.Value.Background);
            Audio.AddItem(WorkingBeatmap.Value.Track);
        }
        
        public void PlayBeatmap()
        {
            WorkingBeatmap.Value.Track.Start();
        }
    }
}