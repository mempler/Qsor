using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace Qsor.Gameplay.osu.Containers
{
    public class PlayfieldContainer : Container
    {
        [Resolved]
        private BeatmapManager BeatmapManager { get; set; }

        public static Vector2 PlayfieldSize => new Vector2(512, 384);

        private double _currentTime;
        protected override void Update()
        {
            if (BeatmapManager.ActiveBeatmap.Track?.IsRunning == false) // Improve performance by not even Updating the HitObjects.
                return;
            
            _currentTime = BeatmapManager.ActiveBeatmap.Track?.CurrentTime + BeatmapManager.ActiveBeatmap.General.AudioLeadIn ?? 0;

            Children // It's faster to iterate through Children. (or should be as there are less objects)
                .OfType<HitObject>() // TODO: remove
                .Where(obj => _currentTime > obj.EndTime)
                .ForEach(obj => obj.Hide());

            BeatmapManager.ActiveBeatmap.HitObjects
                .Where(obj => _currentTime < obj.EndTime)
                .Where(obj => _currentTime > obj.BeginTime - (BeatmapManager.ActiveBeatmap.Difficulty.ApproachRate + 600))
                .Where(obj => !Children.Contains(obj))
                .ForEach(obj =>
                {
                    Add(obj);
                    
                    obj.Show();
                });
        }
    }
}