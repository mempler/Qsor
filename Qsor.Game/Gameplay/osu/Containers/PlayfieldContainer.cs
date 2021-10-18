using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using Qsor.Game.Beatmaps;

namespace Qsor.Game.Gameplay.osu.Containers
{
    public class PlayfieldContainer : Container
    {
        private Bindable<WorkingBeatmap> WorkingBeatmap = new();

        public static Vector2 PlayfieldSize => new(512, 384);

        private double _currentTime;

        private readonly Container _hitObjectLayer;
        
        public PlayfieldContainer()
        {
            InternalChildren = new[]
            {
                _hitObjectLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "HitObject Layer"
                }
            };
        }

        [BackgroundDependencyLoader]
        private void Load(BeatmapManager beatmapManager)
        {
            WorkingBeatmap.BindTo(beatmapManager.WorkingBeatmap);
        }
        
        protected override void Update()
        {
            if (WorkingBeatmap.Value.Track?.IsRunning == false) // Improve performance by not even Updating the HitObjects.
                return;
            
            _currentTime = WorkingBeatmap.Value.Track?.CurrentTime + WorkingBeatmap.Value.General.AudioLeadIn ?? 0;

            _hitObjectLayer // It's faster to iterate through Children. (or should be as there are less objects)
                .OfType<HitObject>() // TODO: remove
                .Where(obj => _currentTime > obj.EndTime)
                .ForEach(obj => obj.Hide());
            
            WorkingBeatmap.Value.HitObjects
                .Where(obj => _currentTime < obj.EndTime)
                .Where(obj => _currentTime > obj.BeginTime - (WorkingBeatmap.Value.Difficulty.ApproachRate + 400))
                .Where(obj => !_hitObjectLayer.Contains(obj)) // Don't duplicate add
                .ForEach(obj =>
                {
                    // obj.TimingPoint = WorkingBeatmap.Value.TimingPoints.FirstOrDefault(s => s.Offset >= _currentTime);
                    
                    _hitObjectLayer.Add(obj);
                    
                    obj.Show();
                });
        }
    }
}