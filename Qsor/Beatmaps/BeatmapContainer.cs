using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using Qsor.Containers;
using Qsor.Gameplay.osu.Containers;

namespace Qsor.Beatmaps
{
    public class BeatmapContainer : Container
    {
        private BackgroundImageContainer _background;
        public WorkingBeatmap WorkingBeatmap { get; }
        
        public PlayfieldContainer Playfield { get; private set; }
        
        [Resolved]
        private AudioManager Audio { get; set; }

        public BeatmapContainer(WorkingBeatmap beatmap)
        {
            WorkingBeatmap = beatmap;
        }
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            FillMode = FillMode.Fill;
            
            LoadComponent(WorkingBeatmap);

            AddInternal(_background = new BackgroundImageContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
            
            _background.SetTexture(WorkingBeatmap.Background);
            Audio.AddItem(WorkingBeatmap.Track);
            
            LoadComponents(WorkingBeatmap.HitObjects); // Preload HitObjects, this makes it twice as fast!
            
            if (Playfield == null)
                AddInternal(new PlayfieldAdjustmentContainer(new PlayfieldContainer{ RelativeSizeAxes = Axes.Both }));
        }
        
        public void PlayBeatmap()
        {
            WorkingBeatmap.Track.Start();
        }
    }
}