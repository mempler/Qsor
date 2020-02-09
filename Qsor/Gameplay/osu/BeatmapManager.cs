using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osuTK;
using Qsor.Containers;
using Qsor.Gameplay.osu.Containers;

namespace Qsor.Gameplay.osu
{
    public class BeatmapManager : Container
    {
        public Beatmap ActiveBeatmap { get; private set; }
        public Track Song { get; private set; }
        public PlayfieldContainer Playfield { get; private set; }

        [Resolved]
        private Storage _Storage { get; set; }
        
        [Resolved]
        private AudioManager _Audio { get; set; }
        
        public BackgroundImageContainer Background;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store, Storage storage)
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            FillMode = FillMode.Fill;
            
            AddInternal(Background = new BackgroundImageContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill,
            });
        }
        
        public void LoadBeatmap(string path)
        {
            ActiveBeatmap = Beatmap.ReadBeatmap(_Storage.GetFullPath(path));
            Background?.SetTexture(ActiveBeatmap.Background);
            
            _Audio.AddItem(Song = new TrackBass(File.OpenRead(ActiveBeatmap.SongFile)));
            
            LoadComponents(ActiveBeatmap.HitObjects); // Preload HitObjects, this makes it twice as fast!
            
            if (Playfield == null)
                AddInternal(Playfield = new PlayfieldContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    FillMode = FillMode.Fit,
                    Scale = new Vector2(2)
                });
        }

        public void PlayBeatmap()
        {
            Song.Start();
        }
    }
}