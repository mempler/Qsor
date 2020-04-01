using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using Qsor.Database;
using Qsor.Database.Models;
using Qsor.Gameplay.osu.Containers;
using Qsor.Graphics.Containers;
using Qsor.Online;

namespace Qsor.Beatmaps
{
    public class BeatmapManager : Component
    {
        public Bindable<WorkingBeatmap> WorkingBeatmap { get; } = new Bindable<WorkingBeatmap>();

        [Resolved]
        private BeatmapMirrorAccess MirrorAccess { get; set; }
        
        [Resolved]
        private Storage Storage { get; set; }
        
        [Resolved]
        private QsorDbContextFactory QsorDbContextFactory { get; set; }

        public BackgroundImageContainer Background;

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            // TODO: Remove
            if (!Storage.ExistsDirectory($"./Songs/{QsorGame.CurrentTestmap}"))
            {
                var beatmapFile = BeatmapMirrorAccess.DownloadBeatmap(QsorGame.CurrentTestmap)
                    .GetAwaiter()
                    .GetResult();
                
                ImportZip(beatmapFile);
            }
        }

        public void ImportZip(Stream beatmapFile)
        {
            using var db = QsorDbContextFactory.GetForWrite();
            using var s = new ZipInputStream(beatmapFile);
            
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                var directoryStorage = Storage.GetStorageForDirectory($"./Songs/{QsorGame.CurrentTestmap}/" + Path.GetDirectoryName(theEntry.Name));
                var fileName = Path.GetFileName(theEntry.Name);
                    
                if (fileName == string.Empty) continue;

                {
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

                if (!fileName.EndsWith(".osu"))
                    continue;
                
                var beatmapFilePath = directoryStorage.GetFullPath(theEntry.Name);
                var beatmap = Beatmap.ReadBeatmap<Beatmap>(directoryStorage, beatmapFilePath);
                    
                db.Context.Beatmaps.Add(new BeatmapModel
                {
                    File = fileName,
                    Audio = beatmap.General.AudioFilename,
                    Path = directoryStorage.GetFullPath(string.Empty),
                    Thumbnail = beatmap.BackgroundFilename
                });
            }
        }
        
        public BeatmapContainer LoadBeatmap(Storage storage, string fileName)
        {
            WorkingBeatmap.Value = Beatmap.ReadBeatmap<WorkingBeatmap>(storage, fileName);
            
            return new BeatmapContainer(WorkingBeatmap);
        }
    }
}