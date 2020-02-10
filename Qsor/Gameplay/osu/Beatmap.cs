using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osuTK;
using osuTK.Graphics;
using Qsor.Gameplay.osu.HitObjects;

namespace Qsor.Gameplay.osu
{
    public class General {
        public string AudioFilename;
        public int AudioLeadIn;
    }

    public class Difficulty {
        public double CircleSize;
        public double ApproachRate;
        public double OverallDifficulty;
        
        
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
    
    public class Beatmap : Container
    {
        public int BeatmapVersion = 0;
        public List<HitObject> HitObjects = new List<HitObject>();
        
        public readonly General General = new General();
        public readonly Difficulty Difficulty = new Difficulty();
        public readonly List<Color4> Colors = new List<Color4>();
        public List<TimingPoint> TimingPoints = new List<TimingPoint>();
        
        private static double LastBPM = 0;

        public const int RulesetId = 0;
        
        // TODO: Figure out.
        //public Track Song { get; private set; }
        public string SongFile { get; private set; }
        public HitObject Last => HitObjects.Last();
        public HitObject First => HitObjects.First();

        public double CurrentElapsed = 0d;
        public bool FreezeBeatmap = false;
        public TimingPoint CurrentTimingPoint;
        public Texture Background;

        private Beatmap() { }
        
        public static Beatmap ReadBeatmap(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Beatmap file hasn't been found!", path);

            var bmDirectory = Path.GetDirectoryName(path);

            var bm = new Beatmap();
            bm.ParseBeatmap(File.ReadAllText(path), path);
            
            bm.SongFile = Path.Combine(bmDirectory, bm.General.AudioFilename);
            
            return bm;
        }
        
        public void ParseBeatmap(string beatmap, string path)
        {
            var bm = beatmap.Split("\n");

            var hitObjects = false;
            var general = false;
            var difficulty = false;
            var color = false;
            var timingPonts = false;
            var events = false;

            var hitObjColIndex = 0;
            var hitObjectColor = SkinManager.SkinColors[0];
            
            foreach (var fln in bm)
            {
                var line = fln.Trim();
                
                if (line.Equals("") || line.Equals(" "))
                {
                    // We've finished reading!
                    hitObjects = false;
                    general = false;
                    difficulty = false;
                    color = false;
                    timingPonts = false;
                    events = false;
                    
                    continue;
                }
                
                if (line.StartsWith("osu file format v")) {
                    var l = line.Split("osu file format v");
                    BeatmapVersion = int.Parse(l[1]);
                }

                if (line.StartsWith("[HitObjects]")) {
                    hitObjects = true;
                    continue;
                }

                if (line.StartsWith("[General]")) {
                    general = true;
                    continue;
                }

                if (line.StartsWith("[Difficulty]")) {
                    difficulty = true;
                    continue;
                }
                
                if (line.StartsWith("[Colours]")) {
                    color = true;
                    continue;
                }
                
                if (line.StartsWith("[TimingPoints]")) {
                    timingPonts = true;
                    continue;
                }
                
                if (line.StartsWith("[Events]")) {
                    events = true;
                    continue;
                }
                
                if (general) {
                    if (line.StartsWith("AudioLeadIn:"))
                        General.AudioLeadIn = int.Parse(line.Split("AudioLeadIn:")[1].Trim());
                    
                    if (line.StartsWith("AudioFilename:"))
                        General.AudioFilename = line.Split("AudioFilename:")[1].Trim();
                }
                
                if (difficulty)
                {
                    if (line.StartsWith("CircleSize:"))
                        Difficulty.CircleSize = double.Parse(line.Split("CircleSize:")[1].Trim());
                    if (line.StartsWith("ApproachRate:"))
                        Difficulty.ApproachRate = double.Parse(line.Split("ApproachRate:")[1].Trim());
                    if (line.StartsWith("OverallDifficulty:"))
                        Difficulty.OverallDifficulty = double.Parse(line.Split("OverallDifficulty:")[1].Trim());
                    if (line.StartsWith("SliderMultiplier:"))
                        Difficulty.SliderMultiplier = double.Parse(line.Split("SliderMultiplier:")[1].Trim());
                }

                if (color)
                {
                    var l = line.Split(":").Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    var rgb = l[1];
                    var col = rgb.Split(",");
                    Colors.Add(new Color4(byte.Parse(col[0]), byte.Parse(col[1]), byte.Parse(col[2]), Byte.MaxValue));
                }

                if (timingPonts)
                {
                    var l = line.Split(",");
                    var timingPoint = new TimingPoint
                    {
                        Offset = double.Parse(l[0].Trim()),
                        MsPerBeat = double.Parse(l[1].Trim()),
                        Meter = int.Parse(l[2].Trim()),
                        SampleSet = int.Parse(l[3].Trim()),
                        SampleIndex = int.Parse(l[4].Trim()),
                        Volume = int.Parse(l[5].Trim()),
                        Inherited = l[6].Trim() == "0", // this is reversed for some fucking reason. FINALLY!!
                        KiaiMode = l[7].Trim() == "1"
                    };
                    timingPoint.BPM = 60000d / timingPoint.MsPerBeat;
   
                    if (timingPoint.Inherited)
                    {
                        timingPoint.SpeedMultiplier = -100 * LastBPM / timingPoint.MsPerBeat;
                    }
                    else
                    {
                        timingPoint.SpeedMultiplier = timingPoint.BPM;
                        LastBPM = timingPoint.SpeedMultiplier;
                    }
  
                    timingPoint.Velocity = Difficulty.SliderMultiplier * timingPoint.SpeedMultiplier / 600f;

                    TimingPoints.Add(timingPoint);
                }

                if (events)
                {
                    var l = line.Split(",");
                    if (l[0].StartsWith("//"))
                        continue;
                    switch (l[0])
                    {
                        case "0":
                            var backgroundPath = Path.GetDirectoryName(path) + "/" +
                                                 l[2] // Remove Quotes 
                                                     .Remove(0, 1)
                                                     .Remove(l[2].Length -2, 1);
                            
                            if (!File.Exists(backgroundPath))
                                continue;

                            using (var fs = File.OpenRead(backgroundPath))
                            {
                                Background = Texture.FromStream(fs);
                            }

                            break;
                        default:
                            Console.WriteLine("Event [{0}] not implemented!", l[0]);
                            break;
                    }
                }
                
                if (hitObjects)
                {
                    CurrentTimingPoint = TimingPoints.FirstOrDefault();
                    
                    var l = line.Split(",");
                    var x = double.Parse(l[0]);
                    var y = double.Parse(l[1]);
                    var timing = int.Parse(l[2]);
                    var hitObjectType = Enum.Parse<HitObjectType>(l[3]);
                    
                    var scale = (float) (0.7 * (Difficulty.CircleSize) / 5);
      
                    if ((hitObjectType & HitObjectType.NewCombo) != 0)
                    {
                        hitObjColIndex++;
                        if (hitObjColIndex >= Colors.Count)
                            hitObjColIndex = 0;
                        
                        if (Colors.Count == 0)
                            Colors.AddRange(SkinManager.SkinColors);
                        
                        hitObjectColor = Colors[hitObjColIndex];
                    }

                    if ((hitObjectType & HitObjectType.Circle) != 0)
                    {
                        HitObject circle = new HitCircle(new Vector2((float) x, (float) y), scale);
                        circle.Beatmap = this;
                        circle.BeginTime = timing;
                        circle.HitObjectColour = hitObjectColor;

                        HitObjects.Add(circle);
                    }
                    
                    if ((hitObjectType & HitObjectType.Slider) != 0)
                    {
                        var sliderInfo = l[5].Split("|");

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

                        var pixelLength = double.Parse(l[7].Trim());
                        var repeats = int.Parse(l[6].Trim());

                        HitObject slider = new HitSlider(
                            sliderType, curvePoints,
                            pixelLength, repeats, scale);

                        slider.Beatmap = this;

                        slider.BeginTime = timing;
                        slider.TimingPoint = TimingPoints.FirstOrDefault(s => s.Offset >= timing);
                        slider.HitObjectColour = hitObjectColor;
                        
                        HitObjects.Add(slider);
                    }
                }
            }
            
            // Sort by Time
            HitObjects.Sort((a, b) => (int) (a.BeginTime - b.BeginTime));

            for (var i = 0; i < HitObjects.Count; i++)
            {
                var obj = HitObjects[i];

                obj.Depth = i;
            }
        }
        
        public void Dispose()
        {
            //Song?.Dispose();
        }
    }
}