using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osuTK;
using Qsor.Beatmaps;

namespace Qsor.Gameplay.osu.Containers
{
    public class PlayfieldContainer : Container
    {
        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }

        public static Vector2 PlayfieldSize => new Vector2(512, 384);

        private double _currentTime;

        private readonly Container _sliderLayer;
        private readonly Container _circleLayer;
        
        public PlayfieldContainer()
        {
            InternalChildren = new[]
            {
                _sliderLayer = new Container(),
                _circleLayer = new Container()
            };
        }
        
        protected override void Update()
        {
            if (BeatmapManager.WorkingBeatmap.Track?.IsRunning == false) // Improve performance by not even Updating the HitObjects.
                return;
            
            _currentTime = BeatmapManager.WorkingBeatmap.Track?.CurrentTime + BeatmapManager.WorkingBeatmap.General.AudioLeadIn ?? 0;

            _sliderLayer // It's faster to iterate through Children. (or should be as there are less objects)
                .OfType<HitObject>() // TODO: remove
                .Where(obj => _currentTime > obj.EndTime)
                .ForEach(obj => obj.Hide());

            _circleLayer // same here.
                .OfType<HitObject>() // TODO: remove
                .Where(obj => _currentTime > obj.EndTime)
                .ForEach(obj => obj.Hide());
            
            BeatmapManager.WorkingBeatmap.HitObjects
                .Where(obj => _currentTime < obj.EndTime)
                .Where(obj => _currentTime > obj.BeginTime - (BeatmapManager.WorkingBeatmap.Difficulty.ApproachRate + 300))
                .Where(obj => !_circleLayer.Contains(obj) && !_sliderLayer.Contains(obj))
                .ForEach(obj =>
                {
                   // obj.TimingPoint = BeatmapManager.ActiveBeatmap.TimingPoints.FirstOrDefault(s => s.Offset >= _currentTime);
                    
                    switch (obj.Type)
                    {
                        case HitObjectType.Circle:
                            _circleLayer.Add(obj);
                            break;
                        case HitObjectType.Slider:
                            _sliderLayer.Add(obj);
                            break;
                        case HitObjectType.NewCombo:
                            break;
                        case HitObjectType.Spinner:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    
                    obj.Show();
                });
        }
    }
}