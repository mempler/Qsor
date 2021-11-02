using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using Qsor.Game.Configuration;
using Qsor.Game.Database;
using Qsor.Game.Database.Models;
using Qsor.Game.Graphics.Containers;
using Qsor.Game.Online;

namespace Qsor.Game.Beatmaps
{
    public class BeatmapManager
    {
        public Bindable<WorkingBeatmap> WorkingBeatmap { get; } = new();

        [Resolved]
        private BeatmapMirrorAccess MirrorAccess { get; set; }
        
        [Resolved]
        private Storage Storage { get; set; }
        
        [Resolved]
        private QsorDbContextFactory QsorDbContextFactory { get; set; }
        
        [Resolved]
        private AudioManager AudioManager { get; set; }
        
        [Resolved]
        private QsorConfigManager ConfigManager { get; set; }

        [Resolved]
        private QsorDbContextFactory DbContextFactory { get; set; }

        public BackgroundImageContainer Background;

        public BeatmapManager()
        {
            WorkingBeatmap.ValueChanged += e =>
            {
                e.OldValue?.Track.Stop();
                e.OldValue?.Dispose();
            };
        }
        
        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            // TODO: Remove
            var setId = ConfigManager.Get<int>(QsorSetting.BeatmapSetId);
            if (!Storage.ExistsDirectory($"./Songs/{setId}"))
            {
                MirrorAccess.DownloadBeatmap(setId);

                ImportStalled();
            }
        }

        /// <summary>
        /// Import all .osz files inside %QSOR_DIR%/Songs/
        /// </summary>
        public void ImportStalled()
        {
            foreach (var osz in Storage.GetFiles("./Songs/", "*.osz"))
            {
                using (var oszStream = Storage.GetStream(osz, FileAccess.Read, FileMode.Open))
                {
                    ImportZip(oszStream);
                }
            
                Storage.Delete(osz); // Cleanup
            }
        }
        
        public void ImportZip(Stream beatmapFile)
        {
            using var db = QsorDbContextFactory.GetForWrite();
            using var s = new ZipInputStream(beatmapFile);
            
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                var directoryStorage = Storage.GetStorageForDirectory($"./Songs/{ConfigManager.Get<int>(QsorSetting.BeatmapSetId)}/" + Path.GetDirectoryName(theEntry.Name));
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

                if (!fileName?.EndsWith(".osu") ?? false)
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

        private readonly List<int> _alreadyRandomized = new();

        public BeatmapContainer NextRandomMap()
        {
            WorkingBeatmap.Value?.Track.Stop();
            
            var ctx = DbContextFactory.Get();
            while (true)
            {
                var beatmapModel = ctx.Beatmaps.Where(s => !_alreadyRandomized.Contains(s.Id))
                    .ToList()
                    .OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefault();

                // Never repeating beatmaps
                if (beatmapModel == null)
                {
                    // We do not have a single beatmap we could use
                    if (_alreadyRandomized.Count <= 0)
                    {
                        return null;
                    }

                    _alreadyRandomized.Clear();
                    continue;
                }

                var beatmapStorage = Storage.GetStorageForDirectory(beatmapModel?.Path);
                _alreadyRandomized.Add(beatmapModel.Id);
                return LoadBeatmap(beatmapStorage, beatmapModel.File);
            }
        }

        public BeatmapContainer PreviousRandomMap()
        {
            var ctx = DbContextFactory.Get();

            while (!ctx.Beatmaps.Any(s => s.Id == _alreadyRandomized.LastOrDefault()))
            {
                if (_alreadyRandomized.Count <= 0)
                {
                    // We don't have any beatmaps to randomize take the next one
                    return NextRandomMap();
                }

                // Beatmap deleted, lets try again
                _alreadyRandomized.RemoveAt(_alreadyRandomized.Count - 1);
            }
 
            _alreadyRandomized.RemoveAt(_alreadyRandomized.Count - 1); // Pop back
            
            var beatmapModel = ctx.Beatmaps
                .FirstOrDefault(s => s.Id == _alreadyRandomized.LastOrDefault());
            
            if (beatmapModel != null)
            {
                var beatmapStorage = Storage.GetStorageForDirectory(beatmapModel.Path);
                return LoadBeatmap(beatmapStorage, beatmapModel.File);
            }

            // Couldn't find the last map, get the next map instead!
            return NextRandomMap();
        }
    }
}