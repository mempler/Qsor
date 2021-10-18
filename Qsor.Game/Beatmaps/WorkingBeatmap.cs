using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;

namespace Qsor.Game.Beatmaps
{
    public class WorkingBeatmap : Beatmap
    {
        private StorageBackedResourceStore _beatmapStore;
        private ITrackStore _trackStore;
        
        public Track Track;
        public Texture Background;
        
        [BackgroundDependencyLoader]
        private void Load(AudioManager audio, QsorBaseGame game)
        {
            _trackStore = audio.GetTrackStore(_beatmapStore = new StorageBackedResourceStore(BeatmapStorage));
            
            // TODO: maybe don't do that.
            Track = _trackStore.Get(General.AudioFilename);
            Background = Texture.FromStream(BeatmapStorage.GetStream(BackgroundFilename));
        }

        public void Play() => Track.Start();

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                base.Dispose(true);
                return;
            }
            
            _trackStore?.Dispose();
            _beatmapStore?.Dispose();
            
            base.Dispose(false);
        }
    }
}