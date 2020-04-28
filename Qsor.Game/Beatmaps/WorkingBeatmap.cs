using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;

namespace Qsor.Game.Beatmaps
{
    public class WorkingBeatmap : Beatmap
    {
        public Track Track;
        public Texture Background;
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            var audioFileStream = BeatmapStorage.GetStream(General.AudioFilename);
            
            Track = new TrackBass(audioFileStream);
            Background = Texture.FromStream(BeatmapStorage.GetStream(BackgroundFilename));
        }

        public void Play()
        {
            if (Track == null)
                throw new BeatmapException("Track is null! is the beatmap not loaded ?");
            
            Track.Start();
        }
    }
}