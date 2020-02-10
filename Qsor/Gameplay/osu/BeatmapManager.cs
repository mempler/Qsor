using System;
using System.IO;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osuTK;
using Qsor.Containers;
using Qsor.Gameplay.osu.Containers;
using Qsor.Online;

namespace Qsor.Gameplay.osu
{
    public class BeatmapManager : Container
    {
        public Beatmap ActiveBeatmap { get; private set; }
        public Track Song { get; private set; }
        public PlayfieldContainer Playfield { get; private set; }

        [Resolved]
        private BeatmapMirrorAccess MirrorAccess { get; set; }
        
        [Resolved]
        private Storage Storage { get; set; }
        
        [Resolved]
        private AudioManager Audio { get; set; }
        
        public BackgroundImageContainer Background;

        [BackgroundDependencyLoader]
        private void Load(BeatmapMirrorAccess mirrorAccess, TextureStore store, Storage storage)
        {
            // TODO: Remove
            if (!storage.ExistsDirectory($"./Songs/{QsorGame.CURRENT_TESTMAP}"))
            {
                var beatmapFile = mirrorAccess.DownloadBeatmap(QsorGame.CURRENT_TESTMAP).GetAwaiter().GetResult();
                
                using var s = new ZipInputStream(beatmapFile);
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    Console.WriteLine(theEntry.Name);

                    var directoryStorage = storage.GetStorageForDirectory($"./Songs/{QsorGame.CURRENT_TESTMAP}/" + Path.GetDirectoryName(theEntry.Name));
                    var fileName = Path.GetFileName(theEntry.Name);
                    
                    if (fileName == string.Empty) continue;
                    
                    using var streamWriter = directoryStorage.GetStream(theEntry.Name, FileAccess.Write);
                    var data = new byte[2048];
                    
                    while (true)
                    {
                        var size = s.Read(data, 0, data.Length);
                        if (size > 0)
                            streamWriter.Write(data, 0, size);
                        else
                            break;
                    }
                }
            }
            
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
            ActiveBeatmap = Beatmap.ReadBeatmap(Storage.GetFullPath(path));
            Background?.SetTexture(ActiveBeatmap.Background);
            
            Audio.AddItem(Song = new TrackBass(File.OpenRead(ActiveBeatmap.SongFile)));
            
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