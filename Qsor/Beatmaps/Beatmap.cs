using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics;
using Qsor.Gameplay;
using Qsor.Gameplay.osu.HitObjects;
using Qsor.Gameplay.osu.HitObjects.Slider;
using Component = osu.Framework.Graphics.Component;

namespace Qsor.Beatmaps
{
    public class General {
        public string AudioFilename;
        public int AudioLeadIn;
    }

    public class Difficulty {
        public double CircleSize = 5;
        public double ApproachRate = 5;
        public double OverallDifficulty = 5;
        
        
        // Sliders
        public double SliderMultiplier;
    }

    public struct TimingPoint
    {
        public double Offset;
        public double MsPerBeat;
        public int Meter;
        public int SampleSet;
        public int SampleIndex;
        public int Volume;
        public bool Inherited;
        public bool KiaiMode;
        
        public double BPM;
        public double Velocity;
        public double SpeedMultiplier;
    }
    
    public class Beatmap : Component
    {
        public int BeatmapVersion = 0;
        public List<HitObject> HitObjects = new List<HitObject>();
        
        public readonly General General = new General();
        public readonly Difficulty Difficulty = new Difficulty();
        public readonly List<Color4> Colors = new List<Color4>();
        public List<TimingPoint> TimingPoints = new List<TimingPoint>();

        public string BackgroundFilename;

        protected Storage BeatmapStorage { get; private set; }

        public static T ReadBeatmap<T>(Storage storage, string fileName)
            where T : Beatmap, new()
        {
            if (!storage.Exists(fileName))
                throw new FileNotFoundException("Beatmap file hasn't been found!", fileName);
            
            var reader = new StreamReader(storage.GetStream(fileName));
            var bmContent = reader.ReadToEnd();

            var bm = new T { BeatmapStorage = storage };
            bm.ConstructFromString(bmContent);

            return bm;
        }
        
        public void ConstructFromString(string content)
        {
            Parse(content);
            
            while (_categoryIndex < _categories.Count)
            {
                switch (GetNextCategory())
                {
                    case Category.General:
                        General.AudioLeadIn = GetValue<int>("AudioLeadIn");
                        General.AudioFilename = GetValue<string>("AudioFilename");
                        break;
                    case Category.Editor:
                        break;
                    case Category.HitObjects:
                        var hitObjectColorIndex = 0;
                        var hitObjectColor = Color4.White;
                        
                        foreach (var hitObjectValue in GetValues().Select(s => s.Split(',')))
                        {
                            var x = double.Parse(hitObjectValue[0]);
                            var y = double.Parse(hitObjectValue[1]);
                            var timing = int.Parse(hitObjectValue[2]);
                            var hitObjectType = Enum.Parse<HitObjectType>(hitObjectValue[3]);
                            
                            if ((hitObjectType & HitObjectType.NewCombo) != 0)
                            {
                                hitObjectColorIndex++;
                                if (hitObjectColorIndex >= Colors.Count)
                                    hitObjectColorIndex = 0;
                                
                                if (Colors.Count == 0)
                                    Colors.AddRange(SkinManager.SkinColors);
                                
                                hitObjectColor = Colors[hitObjectColorIndex];
                            }
                            
                            if ((hitObjectType & HitObjectType.Circle) != 0)
                            {
                                HitObject circle = new HitCircle(this, new Vector2((float) x, (float) y));
                                circle.BeginTime = timing;
                                circle.HitObjectColour = hitObjectColor;

                                HitObjects.Add(circle);
                            }
                            
                            if ((hitObjectType & HitObjectType.Slider) != 0)
                            {
                                var sliderInfo = hitObjectValue[5].Split("|");

                                var sliderType = sliderInfo[0] switch
                                {
                                    "L" => PathType.Linear,
                                    "P" => PathType.PerfectCurve,
                                    "B" => PathType.Bezier,
                                    "C" => PathType.Catmull,
                                    _   => PathType.Linear
                                };

                                var curvePoints = new List<Vector2> {new Vector2((float) x, (float) y)};

                                foreach (var s in sliderInfo)
                                {
                                    if (!s.Contains(":"))
                                        continue;
                                    
                                    var cp = s.Split(":").Select(double.Parse).ToList();

                                    x = cp[0];
                                    y = cp[1];
                                    
                                    var curvePoint = new Vector2((float) x, (float) y);
                                    
                                    curvePoints.Add(curvePoint);
                                }

                                var pixelLength = double.Parse(hitObjectValue[7].Trim());
                                var repeats = int.Parse(hitObjectValue[6].Trim());

                                HitObject slider = new HitSlider(
                                    this,
                                    sliderType, curvePoints,
                                    pixelLength, repeats);

                                slider.BeginTime = timing;
                                slider.TimingPoint = TimingPoints.FirstOrDefault(s => s.Offset >= timing);
                                slider.HitObjectColour = hitObjectColor;
                                
                                HitObjects.Add(slider);
                            }
                        }
                        break;
                    case Category.Difficulty:
                        Difficulty.CircleSize = GetValue<double>("CircleSize");
                        Difficulty.ApproachRate = GetValue<double>("ApproachRate");
                        Difficulty.OverallDifficulty = GetValue<double>("OverallDifficulty");
                        Difficulty.SliderMultiplier = GetValue<double>("SliderMultiplier");
                        break;
                    case Category.Colours:
                        foreach (var value in GetValues())
                        {
                            var colorInformation = value.Split(":").Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                            var col = colorInformation[1].Split(",");
                            
                            Colors.Add(new Color4(byte.Parse(col[0]), byte.Parse(col[1]), byte.Parse(col[2]), byte.MaxValue));
                        }
                        break;
                    case Category.TimingPoints:
                        double lastBpm = 0;
                        
                        foreach (var tPoint in GetValues().Select(s => s.Split(',')))
                        {
                            var timingPoint = new TimingPoint
                            {
                                Offset = double.Parse(tPoint[0].Trim()),
                                MsPerBeat = double.Parse(tPoint[1].Trim()),
                                Meter = int.Parse(tPoint[2].Trim()),
                                SampleSet = int.Parse(tPoint[3].Trim()),
                                SampleIndex = int.Parse(tPoint[4].Trim()),
                                Volume = int.Parse(tPoint[5].Trim()),
                                Inherited = tPoint[6].Trim() == "0", // this is reversed for some fucking reason.
                                KiaiMode = tPoint[7].Trim() == "1"
                            };
                            timingPoint.BPM = 60000d / Math.Clamp(timingPoint.MsPerBeat, 6, 60000);

                            if (timingPoint.Inherited)
                                timingPoint.SpeedMultiplier = -100 * lastBpm / timingPoint.MsPerBeat;
                            else
                                lastBpm = timingPoint.SpeedMultiplier = timingPoint.BPM;
                            
                            timingPoint.Velocity = Difficulty.SliderMultiplier * timingPoint.SpeedMultiplier / 600f;

                            TimingPoints.Add(timingPoint);
                        }
                        break;
                    case Category.Events:
                        foreach (var ev in GetValues().Select(s => s.Split(',')))
                        {
                            if (ev[0].StartsWith("//"))
                                continue;

                            switch (ev[0])
                            {
                                 case "0":
                                     BackgroundFilename =
                                         ev[2].Remove(0, 1) // Remove quotes
                                            .Remove(ev[2].Length -2, 1);
                                     break;
                                 default:
                                     Console.WriteLine("Event [{0}] not implemented!", ev[0]);
                                     break;
                            }
                        }
                        break;
                    case Category.Unknown:
                        Logger.LogPrint("Unknown Category");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            // Sort by Time
            HitObjects.Sort((a, b) => (int) (a.BeginTime - b.BeginTime));

            for (var i = HitObjects.Count - 1; i >= 0; i--)
            {
                var obj = HitObjects[i];
                
                obj.Depth = i;
            }
        }
        
        private int _categoryIndex;
        private readonly List<Category> _categories = new List<Category>();
        private readonly Dictionary<Category, List<string>> _keyValues = new Dictionary<Category, List<string>>();
        
        private void Parse(string content)
        {
            var beatmapLines = content.Split("\n").Select(p => p.Trim());

            var currentValues = new List<string>();
            foreach (var line in beatmapLines)
            {
                if (line == string.Empty) continue;

                if (line.StartsWith("[") && line.EndsWith("]")) {
                    if (!Enum.TryParse(line.Trim('[', ']'), out Category currentCategory))
                        currentCategory = Category.Unknown;
                    
                    _categories.Add(currentCategory);
                    if (!_keyValues.ContainsKey(currentCategory))
                        _keyValues.Add(currentCategory, new List<string>());
                    
                    currentValues = _keyValues[currentCategory];
                    continue;
                }
                
                currentValues.Add(line);
            }
        }

        private Category GetNextCategory() => _categories[_categoryIndex++];
        private Category GetCurrentCategory() => _categories[_categoryIndex -1];
        private IEnumerable<string> GetValues() => _keyValues[GetCurrentCategory()];

        private T GetValue<T>(string key) =>
            GetValues()
                .Where(s => s.ToLower().StartsWith(key.ToLower()))
                .Select(s => s.Split(":").ToArray()[1].TrimStart())
                .Select(s => (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(s))
                .FirstOrDefault();
        
        private enum Category
        {
            Unknown,
            General,
            Editor,
            HitObjects,
            Difficulty,
            Colours,
            TimingPoints,
            Events
        }
    }
}