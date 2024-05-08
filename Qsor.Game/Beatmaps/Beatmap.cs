using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Platform;
using osuTK.Graphics;
using Component = osu.Framework.Graphics.Component;

namespace Qsor.Game.Beatmaps
{
    public partial class Beatmap : Component
    {
        public class GeneralSection
        {
            public string AudioFilename;
            public int AudioLeadIn;
        }

        public class DifficultySection
        {
            public double CircleSize = 5;
            public double ApproachRate = 5;
            public double OverallDifficulty = 5;
            
            // Sliders
            public double SliderMultiplier;
            public double TickRate;
        }
        
        public int BeatmapVersion = 0;
        
        public readonly GeneralSection General = new();
        public readonly DifficultySection Difficulty = new();
        public readonly List<Color4> Colors = new();
        public List<TimingPoint> TimingPoints = new();

        public string BackgroundFilename;

        // TODO: maybe move this to WorkingBeatmap ?
        protected Storage BeatmapStorage { get; private set; }

        public TimingPoint GetTimingPointAt(double time) => TimingPoints.FirstOrDefault(t => t.Offset >= time);

        public static T ReadBeatmap<T>(Storage storage, string fileName)
            where T : Beatmap, new()
        {
            if (!storage.Exists(fileName))
                throw new FileNotFoundException("Beatmap file hasn't been found!", fileName);
            
            var reader = new StreamReader(storage.GetStream(fileName));
            var bmContent = reader.ReadToEnd();

            var parser = new BeatmapParser();
            
            var bm = parser.ConstructFromString<T>(bmContent);
            bm.BeatmapStorage = storage;

            return bm;
        }
    }
}